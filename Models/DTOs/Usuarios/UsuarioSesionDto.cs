using HsqvLogistica.Common;

namespace HsqvLogistica.Models.DTOs.Usuarios
{
    public class UsuarioSesionDto
    {
        public int Id { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public int IdTipo { get; set; }

        public PermisosUsuario Permisos { get; set; } = new();
    }
}
