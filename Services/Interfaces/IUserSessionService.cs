using HsqvLogistica.Models.DTOs.Usuarios;

namespace HsqvLogistica.Services.Interfaces
{
    public interface IUserSessionService
    {
        UsuarioSesionDto? UsuarioActual { get; }

        void IniciarSesion(UsuarioSesionDto usuario);

        void CerrarSesion();

        bool EstaAutenticado { get; }
    }
}
