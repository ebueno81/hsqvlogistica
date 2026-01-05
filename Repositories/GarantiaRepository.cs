using HsqvLogistica.Data;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class GarantiaRepository : IGarantiaRepository
{
    private readonly StoreDbContext _context;

    public GarantiaRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<Garantium?> GetByIdAsync(int id)
    {
        return await _context.Garantia
            .Include(g => g.GarantiaDetalles)
                .ThenInclude(d => d.IdArticuloNavigation)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<(List<Garantium>, int)> SearchAsync(
        string? cliente,
        bool? estado,
        DateOnly? fechaDesde,
        DateOnly? fechaHasta,
        int page,
        int pageSize)
    {
        var query = _context.Garantia
            .Include(g => g.GarantiaDetalles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(cliente))
        {
            query = query.Where(g => g.Cliente!.Contains(cliente));
        }

        if (estado.HasValue)
        {
            query = query.Where(g => g.Estado == estado.Value);
        }

        if (fechaDesde.HasValue)
        {
            query = query.Where(g => g.FechaDespacho >= fechaDesde.Value);
        }

        if (fechaHasta.HasValue)
        {
            query = query.Where(g => g.FechaDespacho <= fechaHasta.Value);
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(g => g.Id)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task AddAsync(Garantium entity)
    {
        await _context.Garantia.AddAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Garantium entity)
    {
        _context.Garantia.Update(entity);
        await _context.SaveChangesAsync();
    }
}
