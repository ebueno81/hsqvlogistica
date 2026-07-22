using HsqvLogistica.Common.Configuration;

namespace HsqvLogistica.Services.Interfaces
{
    public interface IConfigurationService
    {
        Task<string?> GetAsync(string codigo);

        Task<Dictionary<string, string?>> GetAsync(params string[] codigos);

        Task<IReadOnlyDictionary<string, string>> GetAllAsync();

        Task<NotificationSettings> GetNotificationSettingsAsync();

        Task<SmtpSettings> GetSmtpSettingsAsync();

        void LimpiarCache();

    }
}