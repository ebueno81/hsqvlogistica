using HsqvLogistica.Data;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class MotivoRepository : IMotivoRepository
{
    private readonly StoreDbContext _context;

    public MotivoRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Motivo>> GetAllAsync()
        => await _context.Motivos.AsNoTracking().ToListAsync();

    public async Task<Motivo?> GetByIdAsync(int id)
        => await _context.Motivos.FindAsync(id);

    public async Task AddAsync(Motivo motivo)
        => await _context.Motivos.AddAsync(motivo);

    public Task UpdateAsync(Motivo motivo)
    {
        _context.Motivos.Update(motivo);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
