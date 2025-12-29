using HsqvLogistica.Models.DTOs.Articulos;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IArticuloRepository
{
    Task<IEnumerable<ArticuloDto>> GetAllAsync();
    Task<Articulo?> GetByIdAsync(int id);
    Task AddAsync(Articulo articulo);
    Task UpdateAsync(Articulo articulo);
    Task SaveChangesAsync();
}
