using HsqvLogistica.Integrations.Clients.Interfaces;
using HsqvLogistica.Integrations.Models;
using HsqvLogistica.Integrations.Models.Clientes;

namespace HsqvLogistica.Integrations.Clients;

public class EmpresaApiClient : IEmpresaApiClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public EmpresaApiClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<EmpresaDto?> GetEmpresaAsync()
    {
        var endpoint = _config["ExternalApis:SistemaPedidos:EmpresaEndpoint"];

        var response =
            await _http.GetFromJsonAsync<ApiResponse<List<EmpresaDto>>>(endpoint);

        return response?.Value?.FirstOrDefault();
    }
}
