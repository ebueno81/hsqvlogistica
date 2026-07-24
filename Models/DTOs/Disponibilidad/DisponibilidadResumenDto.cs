namespace HsqvLogistica.Models.DTOs.Disponibilidad;

public class DisponibilidadResumenDto
{
    public int IdArticulo { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Articulo { get; set; } = string.Empty;

    public string Linea { get; set; } = string.Empty;

    public decimal StockActual { get; set; }

    public decimal StockFijo { get; set; }

    public int DiasConsultados { get; set; }

    public int DiasCompletos { get; set; }

    public int DiasParciales { get; set; }

    public DateOnly? ProximaFechaDisponible { get; set; }
}