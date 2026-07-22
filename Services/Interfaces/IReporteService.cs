using HsqvLogistica.Models.DTOs.Reportes;

namespace HsqvLogistica.Services.Interfaces;

public interface IReporteService
{
    Task<byte[]> ExportarStockExcelAsync(
        ReporteStockFilterDto filtro);

    Task<byte[]> ExportarKardexExcelAsync(
        ReporteKardexFilterDto filtro);

    Task<byte[]> ExportarGarantiasExcelAsync(
        ReporteGarantiaFilterDto filtro);

    Task<byte[]> ExportarSalidasExcelAsync(
        ReporteKardexFilterDto filtro);

    Task<byte[]> ExportarIngresosExcelAsync(
        ReporteKardexFilterDto filtro);
}