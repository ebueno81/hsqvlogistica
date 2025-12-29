using HsqvLogistica.Integrations.Models;
using HsqvLogistica.Integrations.Models.Clientes;

namespace HsqvLogistica.Integrations.Clients.Interfaces;

public interface IClienteApiClient
{
    Task<List<ClienteLookupDto>> GetClientesAsync(int? idVendedor = null);
    Task<ClienteDetalleDto?> GetByIdAsync(int idCliente);
    Task<List<ClienteLookupDto>> SearchAsync(string filtro);
}
