using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Usuarios;
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

    public async Task<Usuario?> ValidateUserAsync(string user, string pass)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(x =>
                x.Usuario1 == user &&
                x.Clave == pass &&
                x.Activo == true);
    }

    public async Task<UsuarioDto?> GetByUsuarioAsync(string usuario)
    {
        return await _context.Usuarios
            .Where(x => x.Activo == true &&
                        x.Usuario1 == usuario)
            .Select(x => new UsuarioDto
            {
                Id = x.Id,
                Usuario = x.Usuario1,
                Password = x.Clave,
                Nombres = x.Nombres,
                Correo = x.Correo,
                IdTipo = x.IdTipo ?? 0,
                Activo = (bool)x.Activo
            })
            .FirstOrDefaultAsync();
    }
}
