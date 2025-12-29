using HsqvLogistica.Integrations.Models;

namespace HsqvLogistica.Integrations.Clients.Interfaces;

public interface IEmpServApiClient
{
    Task<List<EmpServLookupDto>> GetAllAsync();
}
