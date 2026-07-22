using HsqvLogistica.Common.Configuration;
using HsqvLogistica.Common.Constants;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HsqvLogistica.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguracionRepository _repository;

        private const string CACHE_KEY = "CONFIGURACIONES";

        public ConfigurationService(
            IMemoryCache cache,
            IConfiguracionRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task<IReadOnlyDictionary<string, string>> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync(CACHE_KEY, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                return await _repository.GetDictionaryAsync();
            })
            ?? new Dictionary<string, string>();
        }

        public async Task<string?> GetAsync(string codigo)
        {
            var configuraciones = await GetAllAsync();

            configuraciones.TryGetValue(codigo, out var valor);

            return valor;
        }

        public async Task<Dictionary<string, string?>> GetAsync(params string[] codigos)
        {
            var configuraciones = await GetAllAsync();

            return codigos.ToDictionary(
                codigo => codigo,
                codigo => configuraciones.TryGetValue(codigo, out var valor)
                    ? valor
                    : null);
        }

        public void LimpiarCache()
        {
            _cache.Remove(CACHE_KEY);
        }

        public async Task<NotificationSettings> GetNotificationSettingsAsync()
        {
            var config = await GetAsync(
                ConfiguracionKeys.CorreoSupervisor,
                ConfiguracionKeys.CorreoLogistica,
                ConfiguracionKeys.CorreoAdministrador,
                ConfiguracionKeys.CorreoSistemas,
                ConfiguracionKeys.UrlSistema,
                ConfiguracionKeys.Empresa);

            return new NotificationSettings
            {
                CorreoSupervisor = config[ConfiguracionKeys.CorreoSupervisor],
                CorreoLogistica = config[ConfiguracionKeys.CorreoLogistica],
                CorreoAdministrador = config[ConfiguracionKeys.CorreoAdministrador],
                CorreoSistemas = config[ConfiguracionKeys.CorreoSistemas],
                UrlSistema = config[ConfiguracionKeys.UrlSistema],
                Empresa = config[ConfiguracionKeys.Empresa],
            };
        }

        public async Task<SmtpSettings> GetSmtpSettingsAsync()
        {
            var config = await GetAsync(
                ConfiguracionKeys.SmtpHost,
                ConfiguracionKeys.SmtpPort,
                ConfiguracionKeys.SmtpSsl,
                ConfiguracionKeys.SmtpFrom,
                ConfiguracionKeys.SmtpUser,
                ConfiguracionKeys.SmtpPassword);

            return new SmtpSettings
            {
                Host = config[ConfiguracionKeys.SmtpHost]!,
                Port = int.Parse(config[ConfiguracionKeys.SmtpPort]!),
                EnableSsl = bool.Parse(config[ConfiguracionKeys.SmtpSsl]!),
                From = config[ConfiguracionKeys.SmtpFrom]!,
                Username = config[ConfiguracionKeys.SmtpUser]!,
                Password = config[ConfiguracionKeys.SmtpPassword]!
            };
        }
    }
}