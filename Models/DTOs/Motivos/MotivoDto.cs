namespace HsqvLogistica.Models.DTOs.Motivos;

public class MotivoDto
{
    public int Id { get; set; }
    public string? Descripcion { get; set; }
    public string? TipoMov { get; set; } // ING | SAL
    public bool? Activo { get; set; }
}
