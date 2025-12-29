using HsqvLogistica.Integrations.Auth;
using System.Net.Http.Headers;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly AuthTokenStore _store;

    public AuthTokenHandler(AuthTokenStore store)
    {
        _store = store;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_store.Token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _store.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
