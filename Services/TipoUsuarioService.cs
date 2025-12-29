using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Usuarios;
using HsqvLogistica.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Services
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly StoreDbContext _context;

        public TipoUsuarioService(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoUsuarioLookupDto>> GetLookupAsync()
            => await _context.TipoUsuarios
                .Where(t => t.Activo == true)
                .Select(t => new TipoUsuarioLookupDto
                {
                    Id = t.Id,
                    Descripcion = t.Descripcion
                })
                .ToListAsync();
    }

}
