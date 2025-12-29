using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces;

public interface ILineaRepository
{
    Task<IEnumerable<Linea>> GetAllAsync();
    Task<Linea?> GetByIdAsync(int id);
    Task AddAsync(Linea linea);
    Task UpdateAsync(Linea linea);
    Task SaveChangesAsync();
}
