using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces
{
    public interface IConfiguracionRepository
    {
        Task<List<Configuracion>> GetAllAsync();

        Task<Configuracion?> GetByIdAsync(int id);

        Task<Configuracion?> GetByCodigoAsync(string codigo);

        Task CreateAsync(Configuracion configuracion);

        Task UpdateAsync(Configuracion configuracion);

        Task DeleteAsync(int id);

        Task<Dictionary<string, string>> GetDictionaryAsync();
    }
}
