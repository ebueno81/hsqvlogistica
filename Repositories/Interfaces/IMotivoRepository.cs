using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IMotivoRepository
{
    Task<IEnumerable<Motivo>> GetAllAsync();
    Task<Motivo?> GetByIdAsync(int id);
    Task AddAsync(Motivo motivo);
    Task UpdateAsync(Motivo motivo);
    Task SaveChangesAsync();
}
