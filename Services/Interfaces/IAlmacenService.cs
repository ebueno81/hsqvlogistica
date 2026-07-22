using HsqvLogistica.Models.DTOs.Almacen;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Services.Interfaces;

public interface IAlmacenService
{
    Task<List<AlmacenDto>> GetAllAsync();
    Task<AlmacenDto?> GetByIdAsync(int id);
    Task<AlmacenDto> CreateAsync(AlmacenDto dto);
    Task<bool> UpdateAsync(int id, AlmacenDto dto);
    Task<bool> DeleteAsync(int id);
}
