using HsqvLogistica.Integrations.Auth;
using HsqvLogistica.Integrations.Clients.Interfaces;

namespace HsqvLogistica.Services
{
    public class RemoteLoginService
    {
        private readonly IAuthApiClient _authApi;
        private readonly AuthTokenStore _store;

        public RemoteLoginService(
            IAuthApiClient authApi,
            AuthTokenStore store)
        {
            _authApi = authApi;
            _store = store;
        }

        public async Task EnsureTokenAsync()
        {
            // ✅ Token existe y aún es válido
            if (_store.HasValidToken())
                return;

            // 🔐 Login remoto (credenciales técnicas)
            var token = await _authApi.LoginAsync();

            _store.SetToken(token);
        }
    }
}
