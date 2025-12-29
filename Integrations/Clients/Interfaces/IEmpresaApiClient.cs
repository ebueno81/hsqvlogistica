using HsqvLogistica.Integrations.Models;

namespace HsqvLogistica.Integrations.Clients.Interfaces
{
    public interface IEmpresaApiClient
    {
        Task<EmpresaDto?> GetEmpresaAsync();
    }
}
