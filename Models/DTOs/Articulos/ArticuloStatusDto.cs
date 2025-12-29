namespace HsqvLogistica.Models.DTOs.Articulos
{
    public class ArticuloStatusDto
    {
        public bool Activo { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
}
