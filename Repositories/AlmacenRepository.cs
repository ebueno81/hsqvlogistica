using HsqvLogistica.Data;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class AlmacenRepository : IAlmacenRepository
{
    private readonly StoreDbContext _context;

    public AlmacenRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Almacen>> GetAllAsync()
        => await _context.Almacens.AsNoTracking().ToListAsync();

    public async Task<Almacen?> GetByIdAsync(int id)
        => await _context.Almacens.FindAsync(id);

    public async Task AddAsync(Almacen entity)
    {
        _context.Almacens.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Almacen entity)
    {
        _context.Almacens.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Almacen entity)
    {
        _context.Almacens.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
