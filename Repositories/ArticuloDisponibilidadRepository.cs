using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Disponibilidad;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class ArticuloDisponibilidadRepository
    : IArticuloDisponibilidadRepository
{
    private readonly StoreDbContext _context;

    public ArticuloDisponibilidadRepository(
        StoreDbContext context)
    {
        _context = context;
    }

    public async Task<DisponibilidadResumenDto> ObtenerResumenAsync(
        int idArticulo)
    {
        return await _context.Articulos
            .Where(a => a.Id == idArticulo)
            .Select(a => new DisponibilidadResumenDto
            {
                IdArticulo = a.Id,
                Codigo = a.Codigo,
                Articulo = a.Descripcion,
                Linea = a.IdLineaNavigation.Descripcion,
                StockActual = a.Stock!.Value,
                StockFijo = a.StockFijo!.Value
            })
            .FirstAsync();
    }

    public async Task<List<DisponibilidadCalendarioDto>> ObtenerReservasAsync(
    int idArticulo,
    DateOnly fechaInicio,
    DateOnly fechaFin)
    {
        return await _context.PedidoDetalles

            .Where(d =>
                d.IdArticulo == idArticulo &&
                d.IdPedidoNavigation.Activo == true &&
                (d.IdPedidoNavigation.Estado == 0 ||
                 d.IdPedidoNavigation.Estado == 1) &&
                d.IdPedidoNavigation.FechaEntrega >= fechaInicio &&
                d.IdPedidoNavigation.FechaEntrega <= fechaFin)

            .GroupBy(d => d.IdPedidoNavigation.FechaEntrega)

            .Select(g => new DisponibilidadCalendarioDto
            {
                Fecha = (DateOnly)g.Key,

                CantidadReservada = g.Sum(x => x.Cantidad!.Value)
            })

            .OrderBy(x => x.Fecha)

            .ToListAsync();
    }

    public async Task<List<DisponibilidadDetalleDto>> ObtenerDetalleDiaAsync(
    int idArticulo,
    DateOnly fecha)
    {
        return await _context.PedidoDetalles

            .Where(d =>
                d.IdArticulo == idArticulo &&
                d.IdPedidoNavigation.Activo == true &&
                (d.IdPedidoNavigation.Estado == 0 ||
                 d.IdPedidoNavigation.Estado == 1) &&
                d.IdPedidoNavigation.FechaEntrega == fecha)

            .Select(d => new DisponibilidadDetalleDto
            {
                IdPedido = d.IdPedidoNavigation.Id,

                NumeroPedido = d.IdPedidoNavigation.Id.ToString(),

                Cliente = d.IdPedidoNavigation.Cliente,

                Cantidad = (decimal)d.Cantidad,

                Observacion = d.IdPedidoNavigation.Detalles,

                Estado = d.IdPedidoNavigation.Estado == 0
                    ? "Pendiente"
                    : "Aprobado"
            })

            .OrderBy(x => x.Cliente)

            .ToListAsync();
    }
}