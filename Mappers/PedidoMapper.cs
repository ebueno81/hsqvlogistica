using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Mappers;

public static class PedidoMapper
{
    public static Pedido ToEntity(PedidoCreateDto dto)
        => new()
        {
            IdCliente = dto.IdCliente,
            Cliente = dto.Cliente,
            IdVendedor = dto.IdVendedor,
            FechaEntrega = dto.FechaEntrega,
            Detalles = dto.Detalles,
            Direccion = dto.Direccion,
            IdEmpServ = dto.IdEmpServ,
            Estado = 0,
            Activo = true,
            UsuaCreacion = dto.UsuaCreacion,
            FechaCreacion = DateTime.Now
        };

    public static PedidoDto ToDto(Pedido entity)
        => new()
        {
            Id = entity.Id,
            IdCliente = entity.IdCliente,
            Cliente = entity.Cliente,
            IdVendedor = entity.IdVendedor,
            FechaEntrega = entity.FechaEntrega,
            Detalles = entity.Detalles,
            Direccion = entity.Direccion,
            Estado = entity.Estado,
            Activo = entity.Activo,

             Detalle = entity.PedidoDetalles
            .Select(d => new PedidoDetalleDto
            {
                IdArticulo = d.IdArticulo,
                Articulo = d.IdArticuloNavigation?.Descripcion, // 👈
                Cantidad = (decimal)d.Cantidad
            })
            .ToList()
        };
}
