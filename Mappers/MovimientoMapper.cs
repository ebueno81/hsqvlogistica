using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Mappers;

public static class MovimientoMapper
{
    public static Movimiento ToEntity(MovimientoDto dto)
    {
        return new Movimiento
        {
            IdPedido = dto.IdPedido,
            IdCliente = dto.IdCliente,
            Cliente = dto.Cliente,
            IdAlmacen = dto.IdAlmacen,
            IdMotivo = dto.IdMotivo,
            Fecha = dto.Fecha,
            SerieGuia = dto.SerieGuia,
            NroGuia = dto.NroGuia,
            Detalles = dto.Observacion,
            Activo = dto.Activo,

            UsuaCreacion = dto.Usuario,
            FechaCreacion = DateTime.Now,

            MovimientoDetalles = dto.Detalles
                .Select(x => new MovimientoDetalle
                {
                    IdArticulo = x.IdArticulo,
                    Cantidad = x.Cantidad,
                    Activo = true
                })
                .ToList()
        };
    }

    public static MovimientoDto ToDto(Movimiento entity)
    {
        return new MovimientoDto
        {
            Id = entity.Id,
            IdPedido = entity.IdPedido,
            IdCliente = entity.IdCliente,
            Cliente = entity.Cliente,
            IdAlmacen = entity.IdAlmacen ?? 0,
            IdMotivo = entity.IdMotivo ?? 0,

            Fecha = entity.Fecha ?? DateOnly.MinValue,
            Usuario = entity.UsuaCreacion ?? string.Empty,

            SerieGuia = entity.SerieGuia,
            NroGuia = entity.NroGuia,
            Observacion = entity.Detalles,
            Activo = entity.Activo,

            Tipo = entity.IdMotivoNavigation?.TipoMov ?? string.Empty,
            Motivo = entity.IdMotivoNavigation?.Descripcion ?? string.Empty,

            Detalles = entity.MovimientoDetalles
                .Select(d => new MovimientoDetalleDto
                {
                    IdArticulo = d.IdArticulo ?? 0,
                    Articulo = d.IdArticuloNavigation?.Descripcion ?? string.Empty,
                    Cantidad = d.Cantidad ?? 0
                })
                .ToList()
        };
    }
}
