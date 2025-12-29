using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IAlmacenRepository
{
    Task<List<Almacen>> GetAllAsync();
    Task<Almacen?> GetByIdAsync(int id);
    Task AddAsync(Almacen entity);
    Task UpdateAsync(Almacen entity);
    Task DeleteAsync(Almacen entity);
}
