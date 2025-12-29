using HsqvLogistica.Models.DTOs.Usuarios;

namespace HsqvLogistica.Services.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<UsuarioDto?> GetByIdAsync(int id);
    Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto);
    Task UpdateAsync(int id, UsuarioUpdateDto dto);
    Task<bool> ChangeStatusAsync(int id, bool activo);
}
