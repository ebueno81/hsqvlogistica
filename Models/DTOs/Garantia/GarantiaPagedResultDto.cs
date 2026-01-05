namespace HsqvLogistica.Models.DTOs.Garantias;

public class GarantiaPagedResultDto
{
    public List<GarantiaDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
}
