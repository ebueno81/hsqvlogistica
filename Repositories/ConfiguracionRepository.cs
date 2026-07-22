using HsqvLogistica.Data;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories
{
    public class ConfiguracionRepository : IConfiguracionRepository
    {
        private readonly StoreDbContext _context;

        public ConfiguracionRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<Configuracion>> GetAllAsync()
        {
            return await _context.Configuracions
                .Where(x => x.Activo)
                .OrderBy(x => x.Descripcion)
                .ToListAsync();
        }

        public async Task<Configuracion?> GetByIdAsync(int id)
        {
            return await _context.Configuracions
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Configuracion?> GetByCodigoAsync(string codigo)
        {
            return await _context.Configuracions
                .FirstOrDefaultAsync(x => x.Codigo == codigo && x.Activo);
        }

        public async Task CreateAsync(Configuracion configuracion)
        {
            _context.Configuracions.Add(configuracion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Configuracion configuracion)
        {
            _context.Configuracions.Update(configuracion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Configuracions.FindAsync(id);

            if (entity == null)
                return;

            entity.Activo = false;

            _context.Configuracions.Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync()
        {
            return await _context.Configuracions
                .Where(x => x.Activo)
                .ToDictionaryAsync(
                    x => x.Codigo,
                    x => x.Valor);
        }
    }
}
