using HsqvLogistica.Models.DTOs.Articulos;

namespace HsqvLogistica.Services.Interfaces;

public interface IArticuloService
{
    Task<IEnumerable<ArticuloDto>> GetAllAsync();
    Task<ArticuloDto?> GetByIdAsync(int id);
    Task<ArticuloDto> CreateAsync(ArticuloCreateDto dto);
    Task<bool> UpdateAsync(int id, ArticuloUpdateDto dto);
    Task<bool> ChangeStatusAsync(int id, bool activo, string usuarioModifica);
}
