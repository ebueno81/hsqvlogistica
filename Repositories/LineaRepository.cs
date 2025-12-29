using HsqvLogistica.Data;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class LineaRepository : ILineaRepository
{
    private readonly StoreDbContext _context;

    public LineaRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Linea>> GetAllAsync()
        => await _context.Lineas.ToListAsync();

    public async Task<Linea?> GetByIdAsync(int id)
        => await _context.Lineas.FindAsync(id);

    public async Task AddAsync(Linea linea)
    {
        await _context.Lineas.AddAsync(linea);
    }

    public Task UpdateAsync(Linea linea)
    {
        _context.Lineas.Update(linea);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
