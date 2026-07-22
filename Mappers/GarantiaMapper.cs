using HsqvLogistica.Models.DTOs.Garantia;
using HsqvLogistica.Models.DTOs.Garantias;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Mappers
{
    public class GarantiaMapper
    {
        public GarantiaDto MapToDto(Garantium g)
        {
            return new GarantiaDto
            {
                Id = g.Id,
                Cliente = g.Cliente,
                EmpresaServicio = g.EmpServ,
                FechaDespacho = g.FechaDespacho,
                FechaEntrega = g.FechaEntrega,
                NroGuia = g.NroGuia,
                NroSerie = g.NroSerie,
                Estado = g.Estado,
                Detalles = g.Detalles,
                Activo = g.Activo ?? true,
                FechaModifica = g.FechaModifica,
                UsuaModifica = g.UsuaModifica
            };
        }

        public GarantiaDto MapToDetailDto(Garantium g)
        {
            var dto = MapToDto(g);

            dto.DetallesGarantia = g.GarantiaDetalles
                .Select(x => new GarantiaDetalleDto
                {
                    IdArticulo = x.IdArticulo,
                    Articulo = x.IdArticuloNavigation?.Descripcion ?? "",
                    Cantidad = x.Cantidad ?? 0,
                    Unidad = "UNID.",
                    Detalles = x.Detalles
                }).ToList();

            return dto;
        }
    }
}
