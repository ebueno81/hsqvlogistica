namespace HsqvLogistica.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(
            IEnumerable<string> destinos,
            string asunto,
            string html);
    }
}
