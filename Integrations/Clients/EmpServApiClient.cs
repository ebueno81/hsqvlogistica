using HsqvLogistica.Integrations.Clients.Interfaces;
using HsqvLogistica.Integrations.Models;
using HsqvLogistica.Integrations.Models.Clientes;

namespace HsqvLogistica.Integrations.Clients;

public class EmpServApiClient : IEmpServApiClient
{
    private readonly HttpClient _http;

    public EmpServApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<EmpServLookupDto>> GetAllAsync()
    {
        var response =
            await _http.GetFromJsonAsync<ApiResponse<List<EmpServLookupDto>>>(
                "api/EmpServ/Lista");

        if (response == null || !response.Status)
            return new();

        return response.Value ?? new();
    }
}
