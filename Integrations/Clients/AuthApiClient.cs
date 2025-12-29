namespace HsqvLogistica.Integrations.Clients
{
    using HsqvLogistica.Integrations.Clients.Interfaces;
    using HsqvLogistica.Integrations.Models.Auth;
    using System.Net.Http.Json;

    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AuthApiClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<string> LoginAsync()
        {
            var endpoint =
                _config["ExternalApis:SistemaPedidos:AuthEndpoint"];

            var usuario =
                _config["ExternalApis:SistemaPedidos:ServiceUser:Usuario"];

            var clave =
                _config["ExternalApis:SistemaPedidos:ServiceUser:Clave"];

            if (string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(clave))
                throw new Exception("❌ Credenciales de servicio no configuradas");

            var response = await _http.PostAsJsonAsync(
                endpoint,
                new UsuarioLoginRequestDto
                {
                    Usuario1 = usuario,
                    Clave = clave
                });

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<UsuarioLoginResponseDto>();

            if (result == null || !result.Status || string.IsNullOrWhiteSpace(result.Token))
                throw new Exception("❌ Error obteniendo token");

            return result.Token;
        }
    }
}
