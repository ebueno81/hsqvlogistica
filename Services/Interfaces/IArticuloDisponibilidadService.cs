using HsqvLogistica.Models.DTOs.Disponibilidad;

namespace HsqvLogistica.Services.Interfaces
{
    public interface IArticuloDisponibilidadService
    {
        Task<DisponibilidadArticuloDto> ObtenerDisponibilidadAsync(
            int idArticulo,
            DateOnly fechaInicio,
            DateOnly fechaFin);

        Task<List<DisponibilidadDetalleDto>> ObtenerDetalleDiaAsync(
            int idArticulo,
            DateOnly fecha);
    }
}