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
                Cliente = g.Cliente,                 // 👈 FALTABA
                EmpresaServicio = g.EmpServ, // 👈 FALTABA
                FechaDespacho = g.FechaDespacho,
                FechaEntrega = g.FechaEntrega,
                NroGuia= g.NroGuia,
                NroSerie= g.NroSerie,
                Detalles = g.Detalles,
                Estado = g.Estado,
                //DetallesGarantia = (List<Models.DTOs.Garantia.GarantiaDetalleDto>)g.GarantiaDetalles,
                Activo = g.Activo ?? true
            };
        }
    }
}
