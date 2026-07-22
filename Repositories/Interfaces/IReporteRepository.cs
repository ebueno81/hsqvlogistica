using HsqvLogistica.Models.DTOs.Reportes;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IReporteRepository
{
    Task<List<ReporteStockDto>> ObtenerReporteStockAsync(
        ReporteStockFilterDto filtro);

    Task<List<ReporteKardexDto>> ObtenerReporteKardexAsync(
        ReporteKardexFilterDto filtro);

    Task<List<ReporteGarantiaDto>> ObtenerReporteGarantiasAsync(
        ReporteGarantiaFilterDto filtro);

    Task<List<ReporteSalidaDto>> ObtenerReporteSalidasAsync(
        ReporteKardexFilterDto filtro);

    Task<List<ReporteIngresoDto>> ObtenerReporteIngresosAsync(
        ReporteKardexFilterDto filtro);
}