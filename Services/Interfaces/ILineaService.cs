using HsqvLogistica.Models.DTOs.Lineas;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Services.Interfaces;

public interface ILineaService
{
    // Lectura
    Task<IEnumerable<LineaDto>> GetAllAsync();
    Task<LineaDto?> GetByIdAsync(int id);

    // Escritura
    Task<LineaDto> CreateAsync(LineaCreateDto dto);
    Task<bool> UpdateAsync(int id, LineaUpdateDto dto);

    // Activar / Inactivar
    Task<bool> ChangeStatusAsync(int id, bool activo);
    Task<IEnumerable<LineaDto>> GetActivasAsync();
}
