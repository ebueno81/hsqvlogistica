using HsqvLogistica.Models.DTOs.Disponibilidad;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IArticuloDisponibilidadRepository
{
    Task<DisponibilidadResumenDto> ObtenerResumenAsync(
        int idArticulo);

    Task<List<DisponibilidadCalendarioDto>> ObtenerReservasAsync(
        int idArticulo,
        DateOnly fechaInicio,
        DateOnly fechaFin);

    Task<List<DisponibilidadDetalleDto>> ObtenerDetalleDiaAsync(
        int idArticulo,
        DateOnly fecha);
}