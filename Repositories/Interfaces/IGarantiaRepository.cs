using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IGarantiaRepository
{
    Task<Garantium?> GetByIdAsync(int id);
    Task<(List<Garantium>, int)> SearchAsync(
        string? cliente,
        bool? estado,
        DateOnly? fechaDesde,
        DateOnly? fechaHasta,
        int page,
        int pageSize);

    Task AddAsync(Garantium entity);
    Task SaveChangesAsync();
    Task UpdateAsync(Garantium entity);
}
