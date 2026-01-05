namespace HsqvLogistica.Models.DTOs.Garantias;

public class GarantiaUpdateDto
{
    public int Id { get; set; }
    public DateOnly? FechaDespacho { get; set; }
    public DateOnly? FechaEntrega { get; set; }
    public string? NroSerie { get; set; }
    public string? NroGuia { get; set; }
    public string? Detalles { get; set; }
    public string UsuaModifica { get; set; } = string.Empty;
}

