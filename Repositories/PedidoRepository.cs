using HsqvLogistica.Data;
using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace HsqvLogistica.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly StoreDbContext _context;

    public PedidoRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> GetAllAsync() =>
        await _context.Pedidos
            .Include(p => p.PedidoDetalles)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Pedido?> GetByIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.IdArticuloNavigation) 
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Pedido pedido)
        => await _context.Pedidos.AddAsync(pedido);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<bool> ChangeStatusAsync(int id, int activo, string usuarioModifica)
    {
        var filas = await _context.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC sp_ActualizarEstadoPedido
            @Id={id},
            @Id_Status={activo},
            @Usuario_Modifica={usuarioModifica}");

        return filas > 0;
    }

    public async Task<PedidoPagedResultDto> SearchAsync(
    PedidoFilterDto filter,
    CancellationToken cancellationToken)
    {
        var query = _context.Pedidos.AsQueryable();

        // 🔎 Filtro por cliente
        if (!string.IsNullOrWhiteSpace(filter.Cliente))
        {
            query = query.Where(p =>
                p.Cliente != null &&
                p.Cliente.Contains(filter.Cliente));
        }

        // 📅 Filtro fecha desde
        if (filter.FechaDesde.HasValue)
        {
            var desde = filter.FechaDesde.Value;
            query = query.Where(p => p.FechaEntrega >= desde);
        }

        // 📅 Filtro fecha hasta
        if (filter.FechaHasta.HasValue)
        {
            var hasta = filter.FechaHasta.Value;
            query = query.Where(p => p.FechaEntrega <= hasta);
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.Id)
            .Skip(filter.Page * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => new PedidoDto
            {
                Id = p.Id,
                Cliente = p.Cliente,
                FechaEntrega = p.FechaEntrega,
                Estado = p.Estado,
                Activo = p.Activo
            })
            .ToListAsync(cancellationToken);

        return new PedidoPagedResultDto
        {
            Items = items,
            TotalItems = total
        };
    }

    public async Task<bool> AnularPedido(int id, string usuarioModifica)
    {
        var filas = await _context.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC sp_ActualizarAnularPedido
            @Id={id},
            @Usuario_Modifica={usuarioModifica}");

        return filas > 0;
    }

    public async Task<List<DisponibilidadArticuloDto>> ObtenerDisponibilidadAsync(
        DateOnly fecha, IEnumerable<int> articulos, int? idPedidoExcluir = null)
    {
        var query = _context.PedidoDetalles
             .Where(d =>
                 d.IdArticulo.HasValue &&
                 articulos.Contains(d.IdArticulo.Value) &&
                 d.IdPedidoNavigation.Activo == true &&
                 (d.IdPedidoNavigation.Estado == 0 ||
                  d.IdPedidoNavigation.Estado == 1) &&
                 d.IdPedidoNavigation.FechaEntrega == fecha);

        if (idPedidoExcluir.HasValue)
        {
            query = query.Where(d =>
                d.IdPedido != idPedidoExcluir.Value);
        }

        return await query
            .GroupBy(d => d.IdArticulo)
            .Select(g => new DisponibilidadArticuloDto
            {
                IdArticulo = g.Key!.Value,
                CantidadReservada = (decimal)g.Sum(x => x.Cantidad)
            })
            .ToListAsync();
    }
}
