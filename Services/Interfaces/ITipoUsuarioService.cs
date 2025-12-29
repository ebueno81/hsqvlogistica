using HsqvLogistica.Models.DTOs.Usuarios;

namespace HsqvLogistica.Services.Interfaces
{
    public interface ITipoUsuarioService
    {
        Task<IEnumerable<TipoUsuarioLookupDto>> GetLookupAsync();
    }
}
