using HsqvLogistica.Models.DTOs.Garantias;

namespace HsqvLogistica.Services.Interfaces;

public interface IGarantiaService
{
    Task<GarantiaPagedResultDto> SearchAsync(GarantiaFilterDto filter);
    Task<GarantiaDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(GarantiaCreateDto dto);
    Task UpdateAsync(int id, GarantiaUpdateDto dto);
    Task CloseAsync(int id, string usuario);
    Task AnularAsync(int id, string usuario);
    Task ActivarAsync(int id, string usuario);
}
