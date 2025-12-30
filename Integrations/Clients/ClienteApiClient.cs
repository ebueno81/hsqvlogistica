using HsqvLogistica.Integrations.Clients.Interfaces;
using HsqvLogistica.Integrations.Models;
using HsqvLogistica.Integrations.Models.Clientes;
using HsqvLogistica.Models.DTOs.Clientes;

namespace HsqvLogistica.Integrations.Clients;

public class ClienteApiClient : IClienteApiClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public ClienteApiClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    // ===============================
    // 1️⃣ LISTADO
    // ===============================
    public async Task<List<ClienteLookupDto>> GetClientesAsync(int? idVendedor = null)
    {
        var endpoint = _config["ExternalApis:SistemaPedidos:ClienteEndpoint"]
            ?? throw new Exception("❌ ClienteEndpoint no configurado");

        var url = endpoint;

        if (idVendedor.HasValue)
            url += $"?idVendedor={idVendedor}";

        var response =
            await _http.GetFromJsonAsync<ApiResponse<List<ClienteDetalleDto>>>(url);

        return response?.Value?
            .Select(c => new ClienteLookupDto
            {
                IdCliente = c.IdCliente,
                NombresApellidos = c.NombresApellidos,
                NroDoc = c.NroDoc,
                Celular = c.Celular,
                Distrito = c.Distrito,
                IdVendedor = c.IdVendedor
            })
            .ToList() ?? new();
    }

    // ===============================
    // 2️⃣ DETALLE (OJITO)
    // ===============================
    public async Task<ClienteDetalleDto?> GetByIdAsync(int idCliente)
    {
        var clientes = await GetClientesAsync();
        var c = clientes.FirstOrDefault(c => c.IdCliente == idCliente);
        if (c == null) return null;
        return new ClienteDetalleDto
        {
            IdCliente = c.IdCliente,
            NombresApellidos = c.NombresApellidos,
            NroDoc = c.NroDoc,
            Celular = c.Celular,
            Distrito = c.Distrito,
            IdVendedor = c.IdVendedor
        };
    }

    // ===============================
    // 3️⃣ BUSCAR
    // ===============================
    public async Task<List<ClienteLookupDto>> SearchAsync(string filtro)
    {
        var clientes = await GetClientesAsync();

        return clientes
            .Where(c =>
                (!string.IsNullOrWhiteSpace(c.NombresApellidos) &&
                 c.NombresApellidos.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                ||
                (!string.IsNullOrWhiteSpace(c.NroDoc) &&
                 c.NroDoc.Contains(filtro, StringComparison.OrdinalIgnoreCase))
            )
            .ToList();
    }

    public async Task<List<ClientePedidoDto>> SearchForPedidoAsync(string filtro)
    {
        var clientes = await GetClientesAsync();

        return clientes
            .Where(c =>
                (!string.IsNullOrWhiteSpace(c.NombresApellidos) &&
                 c.NombresApellidos.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                ||
                (!string.IsNullOrWhiteSpace(c.NroDoc) &&
                 c.NroDoc.Contains(filtro, StringComparison.OrdinalIgnoreCase))
            )
            .Select(c => new ClientePedidoDto
            {
                IdCliente = c.IdCliente,
                Nombre = c.NombresApellidos!,
                Documento = c.NroDoc,
                Direccion = $"{c.Distrito}",
                IdVendedor = c.IdVendedor
            })
            .ToList();
    }

}
