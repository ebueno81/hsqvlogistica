using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Services.Interfaces;

public interface IAlmacenService
{
    Task<List<Almacen>> GetAllAsync();
    Task<Almacen?> GetByIdAsync(int id);
    Task<Almacen> CreateAsync(Almacen entity);
    Task<bool> UpdateAsync(int id, Almacen entity);
    Task<bool> DeleteAsync(int id);
}
