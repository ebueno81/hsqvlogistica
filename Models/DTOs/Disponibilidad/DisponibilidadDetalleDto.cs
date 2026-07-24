namespace HsqvLogistica.Models.DTOs.Disponibilidad;

public class DisponibilidadDetalleDto
{
    public int IdPedido { get; set; }

    public string NumeroPedido { get; set; } = string.Empty;

    public string Cliente { get; set; } = string.Empty;

    public decimal Cantidad { get; set; }

    public string? Observacion { get; set; }

    public string Estado { get; set; } = string.Empty;
}