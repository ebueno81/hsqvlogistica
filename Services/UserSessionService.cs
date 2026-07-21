using HsqvLogistica.Models.DTOs.Usuarios;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services
{
    public class UserSessionService : IUserSessionService
    {
        public UsuarioSesionDto? UsuarioActual { get; private set; }

        public bool EstaAutenticado => UsuarioActual != null;

        public void IniciarSesion(UsuarioSesionDto usuario)
        {
            UsuarioActual = usuario;
        }

        public void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
