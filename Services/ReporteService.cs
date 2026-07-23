using ClosedXML.Excel;
using HsqvLogistica.Common.Constants;
using HsqvLogistica.Models.DTOs.Reportes;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class ReporteService : IReporteService
{
    private readonly IReporteRepository _repository;
    private readonly IConfigurationService _configurationService;

    public ReporteService(IReporteRepository repository, IConfigurationService configurationService)
    {
        _repository = repository;
        _configurationService = configurationService;
    }

    public async Task<byte[]> ExportarStockExcelAsync(
    ReporteStockFilterDto filtro)
    {
        var empresa = await _configurationService.GetAsync(
                    ConfiguracionKeys.Empresa);

        var datos = await _repository.ObtenerReporteStockAsync(filtro);

        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Stock");

        // Título
        worksheet.Cell(1, 1).Value = empresa;
        worksheet.Cell(2, 1).Value = "Reporte de Stock";
        worksheet.Cell(3, 1).Value = $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}";

        worksheet.Range("A1:E1").Merge();
        worksheet.Range("A2:E2").Merge();
        worksheet.Range("A3:E3").Merge();

        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 18;

        worksheet.Cell(2, 1).Style.Font.Bold = true;
        worksheet.Cell(2, 1).Style.Font.FontSize = 14;

        // Encabezados
        var fila = 5;

        worksheet.Cell(fila, 1).Value = "Código";
        worksheet.Cell(fila, 2).Value = "Artículo";
        worksheet.Cell(fila, 3).Value = "Línea";
        worksheet.Cell(fila, 4).Value = "Stock";
        worksheet.Cell(fila, 5).Value = "Observación";

        var encabezado = worksheet.Range(fila, 1, fila, 5);

        encabezado.Style.Font.Bold = true;
        encabezado.Style.Fill.BackgroundColor = XLColor.SteelBlue;
        encabezado.Style.Font.FontColor = XLColor.White;
        encabezado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Datos
        fila++;

        foreach (var item in datos)
        {
            worksheet.Cell(fila, 1).Value = item.Codigo;
            worksheet.Cell(fila, 2).Value = item.Articulo;
            worksheet.Cell(fila, 3).Value = item.Linea;
            worksheet.Cell(fila, 4).Value = item.Stock;
            worksheet.Cell(fila, 5).Value = item.Observacion;

            fila++;
        }

        // Bordes
        worksheet.Range(5, 1, fila - 1, 5)
                 .Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        worksheet.Range(5, 1, fila - 1, 5)
                 .Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Autofiltro
        worksheet.Range(5, 1, fila - 1, 5)
                 .SetAutoFilter();

        // Congelar encabezado
        worksheet.SheetView.FreezeRows(5);

        // Ajustar columnas automáticamente
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public async Task<byte[]> ExportarKardexExcelAsync(ReporteKardexFilterDto filtro)
    {
        var datos = await _repository.ObtenerReporteKardexAsync(filtro);

        using var workbook = new XLWorkbook();

        var ws = workbook.Worksheets.Add("Kardex");

        #region Titulo

        ws.Cell("A1").Value = "HSQV LOGÍSTICA";
        ws.Range("A1:L1").Merge();
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A1").Style.Font.FontSize = 18;
        ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        ws.Cell("A2").Value = "REPORTE DE KARDEX DE ARTÍCULOS";
        ws.Range("A2:L2").Merge();
        ws.Cell("A2").Style.Font.Bold = true;
        ws.Cell("A2").Style.Font.FontSize = 15;
        ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        ws.Cell("A3").Value =
            $"Rango de Fechas: {filtro.FechaDesde:dd/MM/yyyy}  al  {filtro.FechaHasta:dd/MM/yyyy}";

        ws.Range("A3:L3").Merge();
        ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Cell("A3").Style.Font.Italic = true;

        #endregion

        #region Encabezados

        ws.Cell(5, 1).Value = "Código";
        ws.Cell(5, 2).Value = "Línea";
        ws.Cell(5, 3).Value = "Artículo";
        ws.Cell(5, 4).Value = "Fecha";
        ws.Cell(5, 5).Value = "Tipo";
        ws.Cell(5, 6).Value = "Motivo";
        ws.Cell(5, 7).Value = "Guía";
        ws.Cell(5, 8).Value = "Cliente";
        ws.Cell(5, 9).Value = "Ingreso";
        ws.Cell(5, 10).Value = "Salida";
        ws.Cell(5, 11).Value = "Saldo";
        ws.Cell(5, 12).Value = "Detalles";

        var header = ws.Range(5, 1, 5, 12);

        header.Style.Font.Bold = true;
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E78");

        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        header.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        header.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        header.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        header.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        #endregion

        #region Datos

        int fila = 6;

        foreach (var item in datos)
        {
            ws.Cell(fila, 1).Value = item.Codigo;
            ws.Cell(fila, 2).Value = item.Linea;
            ws.Cell(fila, 3).Value = item.Articulo;
            ws.Cell(fila, 4).Value = item.Fecha;
            ws.Cell(fila, 5).Value = item.TipoMov;
            ws.Cell(fila, 6).Value = item.Motivo;
            ws.Cell(fila, 7).Value = item.Guia;
            ws.Cell(fila, 8).Value = item.Cliente;
            ws.Cell(fila, 9).Value = item.ING;
            ws.Cell(fila, 10).Value = item.SAL;
            ws.Cell(fila, 11).Value = item.Saldo;
            ws.Cell(fila, 12).Value = item.Detalles;

            fila++;
        }

        #endregion

        #region Formatos

        ws.Column(4).Style.DateFormat.Format = "dd/MM/yyyy";

        ws.Column(9).Style.NumberFormat.Format = "#,##0.00";
        ws.Column(10).Style.NumberFormat.Format = "#,##0.00";
        ws.Column(11).Style.NumberFormat.Format = "#,##0.00";

        ws.Columns().AdjustToContents();

        // Congelar encabezados
        ws.SheetView.FreezeRows(5);

        // Autofiltro
        ws.Range(5, 1, fila - 1, 12).SetAutoFilter();

        #endregion

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public async Task<byte[]> ExportarGarantiasExcelAsync(ReporteGarantiaFilterDto filtro)
    {
        var datos = await _repository.ObtenerReporteGarantiasAsync(filtro);

        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Garantías");

        // ===============================
        // Título
        // ===============================
        worksheet.Cell(1, 1).Value = "HSQV LOGÍSTICA";
        worksheet.Cell(2, 1).Value = "Reporte de Garantías";
        worksheet.Cell(3, 1).Value = $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}";

        worksheet.Range("A1:N1").Merge();
        worksheet.Range("A2:N2").Merge();
        worksheet.Range("A3:N3").Merge();

        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 18;

        worksheet.Cell(2, 1).Style.Font.Bold = true;
        worksheet.Cell(2, 1).Style.Font.FontSize = 14;

        // ===============================
        // Encabezados
        // ===============================
        int fila = 5;

        worksheet.Cell(fila, 1).Value = "N° Garantía";
        worksheet.Cell(fila, 2).Value = "Fecha Despacho";
        worksheet.Cell(fila, 3).Value = "Fecha Entrega";
        worksheet.Cell(fila, 4).Value = "Cliente";
        worksheet.Cell(fila, 5).Value = "Empresa";
        worksheet.Cell(fila, 6).Value = "N° Serie";
        worksheet.Cell(fila, 7).Value = "N° Guía";
        worksheet.Cell(fila, 8).Value = "Código";
        worksheet.Cell(fila, 9).Value = "Artículo";
        worksheet.Cell(fila, 10).Value = "Línea";
        worksheet.Cell(fila, 11).Value = "Cantidad";
        worksheet.Cell(fila, 12).Value = "Detalle";
        worksheet.Cell(fila, 13).Value = "Observación";
        worksheet.Cell(fila, 14).Value = "Estado";

        var encabezado = worksheet.Range(fila, 1, fila, 14);

        encabezado.Style.Font.Bold = true;
        encabezado.Style.Fill.BackgroundColor = XLColor.SteelBlue;
        encabezado.Style.Font.FontColor = XLColor.White;
        encabezado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // ===============================
        // Datos
        // ===============================
        fila++;

        foreach (var item in datos)
        {
            worksheet.Cell(fila, 1).Value = item.Numero;
            worksheet.Cell(fila, 2).Value = item.FechaDespacho.ToString();
            worksheet.Cell(fila, 3).Value = item.FechaEntrega.ToString();
            worksheet.Cell(fila, 4).Value = item.Cliente;
            worksheet.Cell(fila, 5).Value = item.EmpresaServicio;
            worksheet.Cell(fila, 6).Value = item.NumeroSerie;
            worksheet.Cell(fila, 7).Value = item.NumeroGuia;
            worksheet.Cell(fila, 8).Value = item.CodigoArticulo;
            worksheet.Cell(fila, 9).Value = item.Articulo;
            worksheet.Cell(fila, 10).Value = item.Linea;
            worksheet.Cell(fila, 11).Value = item.Cantidad;
            worksheet.Cell(fila, 12).Value = item.DetalleArticulo;
            worksheet.Cell(fila, 13).Value = item.Observacion;
            worksheet.Cell(fila, 14).Value = item.Estado;

            fila++;
        }

        // ===============================
        // Formato
        // ===============================
        worksheet.Columns(2, 3).Style.DateFormat.Format = "dd/MM/yyyy";

        worksheet.Column(11).Style.NumberFormat.Format = "#,##0.00";

        worksheet.Range(5, 1, fila - 1, 14)
            .Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        worksheet.Range(5, 1, fila - 1, 14)
            .Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        worksheet.Range(5, 1, fila - 1, 14)
            .SetAutoFilter();

        worksheet.SheetView.FreezeRows(5);

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public async Task<byte[]> ExportarSalidasExcelAsync(
        ReporteKardexFilterDto filtro)
    {
        var datos = await _repository.ObtenerReporteSalidasAsync(filtro);

        using var workbook = new XLWorkbook();

        var ws = workbook.Worksheets.Add("Salidas");

        #region Título

        ws.Cell("A1").Value = ConfiguracionKeys.Empresa;
        ws.Range("A1:I1").Merge();
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A1").Style.Font.FontSize = 18;
        ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        ws.Cell("A2").Value = "REPORTE DE SALIDAS";
        ws.Range("A2:I2").Merge();
        ws.Cell("A2").Style.Font.Bold = true;
        ws.Cell("A2").Style.Font.FontSize = 15;
        ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        ws.Cell("A3").Value =
            $"Rango de Fechas: {filtro.FechaDesde:dd/MM/yyyy} al {filtro.FechaHasta:dd/MM/yyyy}";

        ws.Range("A3:I3").Merge();
        ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Cell("A3").Style.Font.Italic = true;

        #endregion

        #region Encabezados

        ws.Cell(5, 1).Value = "Código";
        ws.Cell(5, 2).Value = "Línea";
        ws.Cell(5, 3).Value = "Artículo";
        ws.Cell(5, 4).Value = "Fecha";
        ws.Cell(5, 5).Value = "Motivo";
        ws.Cell(5, 6).Value = "Guía";
        ws.Cell(5, 7).Value = "Cliente";
        ws.Cell(5, 8).Value = "Cantidad";
        ws.Cell(5, 9).Value = "Detalles";

        var header = ws.Range(5, 1, 5, 9);

        header.Style.Font.Bold = true;
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#C00000");

        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        header.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        header.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        header.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        header.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        #endregion

        #region Datos

        int fila = 6;

        foreach (var item in datos)
        {
            ws.Cell(fila, 1).Value = item.Codigo;
            ws.Cell(fila, 2).Value = item.Linea;
            ws.Cell(fila, 3).Value = item.Articulo;
            ws.Cell(fila, 4).Value = item.Fecha;
            ws.Cell(fila, 5).Value = item.Motivo;
            ws.Cell(fila, 6).Value = item.Guia;
            ws.Cell(fila, 7).Value = item.Cliente;
            ws.Cell(fila, 8).Value = item.Cantidad;
            ws.Cell(fila, 9).Value = item.Detalles;

            fila++;
        }

        #endregion

        #region Formatos

        ws.Column(4).Style.DateFormat.Format = "dd/MM/yyyy";
        ws.Column(8).Style.NumberFormat.Format = "#,##0.00";

        ws.Columns().AdjustToContents();

        ws.SheetView.FreezeRows(5);

        ws.Range(5, 1, fila - 1, 9).SetAutoFilter();

        #endregion

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public async Task<byte[]> ExportarIngresosExcelAsync(ReporteKardexFilterDto filtro)
    {
        var datos = await _repository.ObtenerReporteIngresosAsync(filtro);

        using var workbook = new XLWorkbook();

        var ws = workbook.Worksheets.Add("Ingresos");

        #region Título

        ws.Cell("A1").Value = ConfiguracionKeys.Empresa;
        ws.Range("A1:I1").Merge();
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A1").Style.Font.FontSize = 18;
        ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        ws.Cell("A2").Value = "REPORTE DE INGRESOS";
        ws.Range("A2:I2").Merge();
        ws.Cell("A2").Style.Font.Bold = true;
        ws.Cell("A2").Style.Font.FontSize = 15;
        ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        ws.Cell("A3").Value =
            $"Rango de Fechas: {filtro.FechaDesde:dd/MM/yyyy} al {filtro.FechaHasta:dd/MM/yyyy}";

        ws.Range("A3:I3").Merge();
        ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Cell("A3").Style.Font.Italic = true;

        #endregion

        #region Encabezados

        ws.Cell(5, 1).Value = "Código";
        ws.Cell(5, 2).Value = "Línea";
        ws.Cell(5, 3).Value = "Artículo";
        ws.Cell(5, 4).Value = "Fecha";
        ws.Cell(5, 5).Value = "Motivo";
        ws.Cell(5, 6).Value = "Guía";
        ws.Cell(5, 7).Value = "Cliente";
        ws.Cell(5, 8).Value = "Cantidad";
        ws.Cell(5, 9).Value = "Detalles";

        var header = ws.Range(5, 1, 5, 9);

        header.Style.Font.Bold = true;
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#548235");

        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        header.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        header.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        header.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        header.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        #endregion

        #region Datos

        int fila = 6;

        foreach (var item in datos)
        {
            ws.Cell(fila, 1).Value = item.Codigo;
            ws.Cell(fila, 2).Value = item.Linea;
            ws.Cell(fila, 3).Value = item.Articulo;
            ws.Cell(fila, 4).Value = item.Fecha;
            ws.Cell(fila, 5).Value = item.Motivo;
            ws.Cell(fila, 6).Value = item.Guia;
            ws.Cell(fila, 7).Value = item.Cliente;
            ws.Cell(fila, 8).Value = item.Cantidad;
            ws.Cell(fila, 9).Value = item.Detalles;

            fila++;
        }

        #endregion

        #region Formatos

        ws.Column(4).Style.DateFormat.Format = "dd/MM/yyyy";
        ws.Column(8).Style.NumberFormat.Format = "#,##0.00";

        ws.Columns().AdjustToContents();

        ws.SheetView.FreezeRows(5);

        ws.Range(5, 1, fila - 1, 9).SetAutoFilter();

        #endregion

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}