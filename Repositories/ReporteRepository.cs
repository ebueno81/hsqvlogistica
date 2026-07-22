using DocumentFormat.OpenXml.Wordprocessing;
using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Reportes;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace HsqvLogistica.Repositories;

public class ReporteRepository : IReporteRepository
{
    private readonly StoreDbContext _context;

    public ReporteRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReporteStockDto>> ObtenerReporteStockAsync(
    ReporteStockFilterDto filtro)
    {
        var query =
            _context.Articulos
            .Include(x => x.IdLineaNavigation)
            .AsQueryable();

        if (filtro.IdLinea.HasValue)
        {
            query = query.Where(x =>
                x.IdLinea == filtro.IdLinea.Value);
        }

        var datos =
            await query
            .OrderBy(x => x.IdLineaNavigation.Descripcion)
            .ThenBy(x => x.Descripcion)
            .Select(x => new ReporteStockDto
            {
                Codigo = x.Codigo,
                Articulo = x.Descripcion,
                Linea = x.IdLineaNavigation.Descripcion,
                Stock = x.Stock ?? 0,
                Observacion = x.Detalles
            })
            .ToListAsync();

        return datos;
    }

    public async Task<List<ReporteKardexDto>> ObtenerReporteKardexAsync(
            ReporteKardexFilterDto filtro)
    {
        try
        {
            return await _context.ReporteKardex
            .FromSqlInterpolated($@"
            EXEC dbo.sp_KardexArticulos
                @FechaInicio={filtro.FechaDesde},
                @FechaFin={filtro.FechaHasta},
                @IdAlmacen={filtro.IdAlmacen},
                @IdLinea={filtro.IdLinea},
                @IdArticulo={filtro.IdArticulo}")
            .AsNoTracking()
            .ToListAsync();
        } catch (Exception ex)
        {
            // Manejar la excepción según sea necesario
            throw new Exception("Error al ejecutar el procedimiento almacenado sp_KardexArticulos.", ex);
        }
    }

    public async Task<List<ReporteGarantiaDto>> ObtenerReporteGarantiasAsync(
        ReporteGarantiaFilterDto filtro)
    {
        var query = _context.GarantiaDetalles
            .AsNoTracking()
            .Include(x => x.IdGarantiaNavigation)
            .Include(x => x.IdArticuloNavigation)
                .ThenInclude(x => x.IdLineaNavigation)
            .AsQueryable();

        if (filtro.FechaDesde != null)
        {
            query = query.Where(x =>
                x.IdGarantiaNavigation.FechaDespacho >=
                DateOnly.FromDateTime(filtro.FechaDesde));
        }

        if (filtro.FechaHasta != null)
        {
            query = query.Where(x =>
                x.IdGarantiaNavigation.FechaDespacho <=
                DateOnly.FromDateTime(filtro.FechaHasta));
        }

        if (filtro.Estado.HasValue)
        {
            query = query.Where(x =>
                x.IdGarantiaNavigation.Estado == filtro.Estado);
        }

        return await query
            .Select(x => new ReporteGarantiaDto
            {
                Numero = x.IdGarantiaNavigation.Id,

                FechaDespacho = x.IdGarantiaNavigation.FechaDespacho,

                FechaEntrega = x.IdGarantiaNavigation.FechaEntrega,

                Cliente = x.IdGarantiaNavigation.Cliente,

                EmpresaServicio = x.IdGarantiaNavigation.EmpServ,

                NumeroSerie = x.IdGarantiaNavigation.NroSerie,

                NumeroGuia = x.IdGarantiaNavigation.NroGuia,

                Observacion = x.IdGarantiaNavigation.Detalles,

                Estado = x.IdGarantiaNavigation.Estado
                            ? "Cerrada"
                            : "Pendiente",

                CodigoArticulo = x.IdArticuloNavigation.Codigo,

                Articulo = x.IdArticuloNavigation.Descripcion,

                Linea = x.IdArticuloNavigation
                            .IdLineaNavigation
                            .Descripcion,

                Cantidad = x.Cantidad ?? 0,

                DetalleArticulo = x.Detalles
            })
            .OrderBy(x => x.Numero)
            .ThenBy(x => x.Articulo)
            .ToListAsync();
    }

    public async Task<List<ReporteSalidaDto>> ObtenerReporteSalidasAsync(
        ReporteKardexFilterDto filtro)
    {
        var fechaDesde = DateOnly.FromDateTime(filtro.FechaDesde);
        var fechaHasta = DateOnly.FromDateTime(filtro.FechaHasta);

        var query = _context.MovimientoDetalles
            .Where(d =>
                d.Activo == true &&
                d.IdMovNavigation != null &&
                d.IdMovNavigation.Activo == true &&
                d.IdMovNavigation.IdAlmacen == filtro.IdAlmacen &&
                d.IdMovNavigation.Fecha.HasValue &&
                d.IdMovNavigation.Fecha >= fechaDesde &&
                d.IdMovNavigation.Fecha <= fechaHasta &&
                d.IdMovNavigation.IdMotivoNavigation != null &&
                d.IdMovNavigation.IdMotivoNavigation.TipoMov == "SAL");

        if (filtro.IdArticulo.HasValue)
            query = query.Where(x => x.IdArticulo == filtro.IdArticulo);

        if (filtro.IdLinea.HasValue)
            query = query.Where(x => x.IdArticuloNavigation.IdLinea == filtro.IdLinea);

        try
        {
            return await query

            .OrderBy(x => x.IdArticuloNavigation.Descripcion)
            .ThenBy(x => x.IdMovNavigation.Fecha)

            .Select(x => new ReporteSalidaDto
            {
                Codigo = x.IdArticuloNavigation.Codigo,

                Linea = x.IdArticuloNavigation.IdLineaNavigation.Descripcion,

                Articulo = x.IdArticuloNavigation.Descripcion,

                Fecha = (DateTime)(x.IdMovNavigation.Fecha.HasValue
                ? x.IdMovNavigation.Fecha.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null),

                Motivo = x.IdMovNavigation.IdMotivoNavigation.Descripcion,

                Guia =
                    (x.IdMovNavigation.SerieGuia ?? "") +
                    (
                        string.IsNullOrWhiteSpace(x.IdMovNavigation.NroGuia)
                        ? ""
                        : "-" + x.IdMovNavigation.NroGuia
                    ),

                Cliente = x.IdMovNavigation.Cliente,

                Cantidad = (decimal)x.Cantidad,

                Detalles = x.IdMovNavigation.Detalles

            })
            .ToListAsync();
        } catch (Exception ex)
        {
            // Manejar la excepción según sea necesario
            throw new Exception("Error al obtener el reporte de salidas.", ex);
        }
    }

    public async Task<List<ReporteIngresoDto>> ObtenerReporteIngresosAsync(
        ReporteKardexFilterDto filtro)
    {
        var fechaDesde = DateOnly.FromDateTime(filtro.FechaDesde);
        var fechaHasta = DateOnly.FromDateTime(filtro.FechaHasta);

        var query = _context.MovimientoDetalles
            .Where(d =>
                d.Activo == true &&
                d.IdMovNavigation != null &&
                d.IdMovNavigation.Activo == true &&
                d.IdMovNavigation.IdAlmacen == filtro.IdAlmacen &&
                d.IdMovNavigation.Fecha >= fechaDesde &&
                d.IdMovNavigation.Fecha <= fechaHasta &&
                d.IdMovNavigation.IdMotivoNavigation != null &&
                d.IdMovNavigation.IdMotivoNavigation.TipoMov == "ING");

        if (filtro.IdArticulo.HasValue)
            query = query.Where(x => x.IdArticulo == filtro.IdArticulo);

        if (filtro.IdLinea.HasValue)
            query = query.Where(x => x.IdArticuloNavigation.IdLinea == filtro.IdLinea);

        return await query
            .OrderBy(x => x.IdArticuloNavigation.Descripcion)
            .ThenBy(x => x.IdMovNavigation.Fecha)
            .Select(x => new ReporteIngresoDto
            {
                Codigo = x.IdArticuloNavigation.Codigo,

                Linea = x.IdArticuloNavigation.IdLineaNavigation.Descripcion,

                Articulo = x.IdArticuloNavigation.Descripcion,

                Fecha = (DateTime)(x.IdMovNavigation.Fecha.HasValue
                ? x.IdMovNavigation.Fecha.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null),

                Motivo = x.IdMovNavigation.IdMotivoNavigation.Descripcion,

                Guia =
                    (x.IdMovNavigation.SerieGuia ?? "") +
                    (
                        x.IdMovNavigation.NroGuia == null ||
                        x.IdMovNavigation.NroGuia == ""
                        ? ""
                        : "-" + x.IdMovNavigation.NroGuia
                    ),

                Cliente = x.IdMovNavigation.Cliente,

                Cantidad = x.Cantidad ?? 0,

                Detalles = x.IdMovNavigation.Detalles
            })
            .ToListAsync();
    }
}