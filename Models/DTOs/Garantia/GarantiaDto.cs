using HsqvLogistica.Models.DTOs.Garantia;

namespace HsqvLogistica.Models.DTOs.Garantias;

public class GarantiaDto
{
    public int Id { get; set; }

    public int IdCliente { get; set; }
    public string? Cliente { get; set; }

    public int IdEmpServ { get; set; }
    public string? EmpresaServicio { get; set; }

    public DateOnly? FechaDespacho { get; set; }
    public DateOnly? FechaEntrega { get; set; }

    public string? NroSerie { get; set; }
    public string? NroGuia { get; set; }

    public bool Estado { get; set; } // 0 Pendiente | 1 Cerrado
    public bool Activo { get; set; }

    public string? Detalles { get; set; }

    public List<GarantiaDetalleDto> DetallesGarantia { get; set; } = new();
}
