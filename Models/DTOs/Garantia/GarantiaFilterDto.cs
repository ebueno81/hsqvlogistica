namespace HsqvLogistica.Models.DTOs.Garantias;

public class GarantiaFilterDto
{
    public string? Cliente { get; set; }
    public bool? Estado { get; set; }
    public DateOnly? FechaDesde { get; set; }
    public DateOnly? FechaHasta { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
}
