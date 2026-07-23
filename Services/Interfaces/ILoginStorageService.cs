using HsqvLogistica.Models.DTOs.Usuarios;

namespace HsqvLogistica.Services.Interfaces
{
    public interface ILoginStorageService
    {
        Task GuardarAsync(LoginRememberDto model);

        Task<LoginRememberDto?> ObtenerAsync();

        Task EliminarAsync();
    }
}
