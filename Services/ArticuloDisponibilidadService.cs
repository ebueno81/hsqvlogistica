using HsqvLogistica.Models.DTOs.Disponibilidad;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class ArticuloDisponibilidadService
    : IArticuloDisponibilidadService
{
    private readonly IArticuloDisponibilidadRepository _repository;

    public ArticuloDisponibilidadService(
        IArticuloDisponibilidadRepository repository)
    {
        _repository = repository;
    }

    public async Task<DisponibilidadArticuloDto> ObtenerDisponibilidadAsync(
        int idArticulo,
        DateOnly fechaInicio,
        DateOnly fechaFin)
    {
        var resumen = await _repository.ObtenerResumenAsync(idArticulo);

        var reservas = await _repository.ObtenerReservasAsync(
            idArticulo,
            fechaInicio,
            fechaFin);

        var calendario = GenerarCalendarioCompleto(
            reservas,
            fechaInicio,
            fechaFin,
            resumen.StockFijo);

        CalcularResumen(
            resumen,
            calendario);

        return new DisponibilidadArticuloDto
        {
            Resumen = resumen,
            Calendario = calendario
        };
    }

    public async Task<List<DisponibilidadDetalleDto>> ObtenerDetalleDiaAsync(
        int idArticulo,
        DateOnly fecha)
    {
        return await _repository.ObtenerDetalleDiaAsync(
            idArticulo,
            fecha);
    }

    private List<DisponibilidadCalendarioDto> GenerarCalendarioCompleto(
        List<DisponibilidadCalendarioDto> reservas,
        DateOnly fechaInicio,
        DateOnly fechaFin,
        decimal stockFijo)
    {
        var calendario = new List<DisponibilidadCalendarioDto>();

        for (var fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
        {
            var reserva = reservas.FirstOrDefault(x => x.Fecha == fecha);

            var reservado = reserva?.CantidadReservada ?? 0;

            calendario.Add(new DisponibilidadCalendarioDto
            {
                Fecha = fecha,
                StockFijo = stockFijo,
                CantidadReservada = reservado,
                CantidadDisponible = stockFijo - reservado,
                PorcentajeReservado = stockFijo == 0
                    ? 0
                    : reservado * 100 / stockFijo
            });
        }

        return calendario;
    }

    private void CalcularResumen(
        DisponibilidadResumenDto resumen,
        List<DisponibilidadCalendarioDto> calendario)
    {
        resumen.DiasConsultados = calendario.Count;

        resumen.DiasCompletos = calendario.Count(x =>
            x.CantidadDisponible <= 0);

        resumen.DiasParciales = calendario.Count(x =>
            x.CantidadReservada > 0 &&
            x.CantidadDisponible > 0);

        resumen.ProximaFechaDisponible = calendario
            .Where(x => x.CantidadDisponible > 0)
            .OrderBy(x => x.Fecha)
            .Select(x => (DateOnly?)x.Fecha)
            .FirstOrDefault();
    }
}