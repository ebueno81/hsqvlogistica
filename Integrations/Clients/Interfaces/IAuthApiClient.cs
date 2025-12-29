namespace HsqvLogistica.Integrations.Clients.Interfaces
{
    public interface IAuthApiClient
    {
        Task<string> LoginAsync();
    }
}
