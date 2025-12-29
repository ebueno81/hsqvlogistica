using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Articulos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ArticuloRepository : IArticuloRepository
{
    private readonly StoreDbContext _context;

    public ArticuloRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ArticuloDto>> GetAllAsync()
    {
        return await _context.Articulos
            .AsNoTracking()
            .Include(a => a.IdLineaNavigation)
            .Select(a => new ArticuloDto
            {
                Id = a.Id,
                Codigo = a.Codigo,
                IdLinea = a.IdLinea,
                LineaDescripcion = a.IdLineaNavigation!.Descripcion,

                Descripcion = a.Descripcion,
                Stock = a.Stock,
                PrecioMn = a.PrecioMn,
                PrecioUs = a.PrecioUs,
                RutaImagen = a.RutaImagen,
                Activo = a.Activo
            })
            .ToListAsync();
    }

    public async Task<Articulo?> GetByIdAsync(int id)
        => await _context.Articulos.FindAsync(id);

    public async Task AddAsync(Articulo articulo)
        => await _context.Articulos.AddAsync(articulo);

    public Task UpdateAsync(Articulo articulo)
    {
        _context.Articulos.Update(articulo);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
