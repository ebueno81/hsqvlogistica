using HsqvLogistica.Models.DTOs.Motivos;

namespace HsqvLogistica.Services.Interfaces;

public interface IMotivoService
{
    Task<IEnumerable<MotivoDto>> GetAllAsync();
    Task<MotivoDto?> GetByIdAsync(int id);
    Task<MotivoDto> CreateAsync(MotivoCreateDto dto);
    Task UpdateAsync(int id, MotivoUpdateDto dto);
    Task ToggleAsync(int id);
}
