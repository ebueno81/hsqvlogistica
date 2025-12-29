using HsqvLogistica.Data;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly StoreDbContext _context;

    public UsuarioRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync() =>
        await _context.Usuarios
            .Include(u => u.IdTipoNavigation)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Usuario?> GetByIdAsync(int id) =>
        await _context.Usuarios
            .Include(u => u.IdTipoNavigation)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task AddAsync(Usuario usuario) =>
        await _context.Usuarios.AddAsync(usuario);

    public Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

}
