namespace HsqvLogistica.Models.DTOs.Reportes;

public class ReporteGarantiaDto
{
    // Cabecera
    public int Numero { get; set; }

    public DateOnly? FechaDespacho { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public string EmpresaServicio { get; set; } = string.Empty;

    public string NumeroSerie { get; set; } = string.Empty;

    public string NumeroGuia { get; set; } = string.Empty;

    public string Observacion { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    // Detalle
    public string CodigoArticulo { get; set; } = string.Empty;

    public string Articulo { get; set; } = string.Empty;

    public string Linea { get; set; } = string.Empty;

    public decimal Cantidad { get; set; }

    public string DetalleArticulo { get; set; } = string.Empty;
}