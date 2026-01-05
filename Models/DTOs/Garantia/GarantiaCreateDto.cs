using HsqvLogistica.Models.DTOs.Garantia;

namespace HsqvLogistica.Models.DTOs.Garantias;

public class GarantiaCreateDto
{
    public int IdCliente { get; set; }
    public int IdEmpServ { get; set; }

    public DateOnly? FechaDespacho { get; set; }
    public DateOnly? FechaEntrega { get; set; }

    public string? NroSerie { get; set; }
    public string? NroGuia { get; set; }
    public string? Cliente { get; set; }
    public string? EmpServ { get; set; }

    public string? Detalles { get; set; }

    public List<GarantiaDetalleDto> GarantiaDetalles { get; set; } = new();

    public string UsuaCreacion { get; set; } = string.Empty;
}
