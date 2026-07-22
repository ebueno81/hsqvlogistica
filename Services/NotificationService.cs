using HsqvLogistica.Mappers;
using HsqvLogistica.Repositories;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly IConfigurationService _configurationService;
        private readonly ITemplateService _templateService;
        private readonly IEmailService _emailService;
        private readonly IGarantiaRepository _garantiaRepository;
        private readonly GarantiaMapper _garantiaMapper;

        public NotificationService(
            IPedidoRepository pedidoRepository,
            IMovimientoRepository movimientoRepository,
            IConfigurationService configurationService,
            ITemplateService templateService,
            IEmailService emailService,
            IGarantiaRepository garantiaRepository,
            GarantiaMapper garantiaMapper)
        {
            _pedidoRepository = pedidoRepository;
            _movimientoRepository = movimientoRepository;
            _configurationService = configurationService;
            _templateService = templateService;
            _emailService = emailService;
            _garantiaRepository = garantiaRepository;
            _garantiaMapper = garantiaMapper;
        }

        public async Task NotificarPedidoCreadoAsync(int idPedido)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(idPedido);
            
            if (pedido == null)
                return;

            var pedidoDto = PedidoMapper.ToDto(pedido);

            var settings =
                await _configurationService.GetNotificationSettingsAsync();

            var html =
                await _templateService.PedidoCreadoAsync(pedidoDto, settings);

            await _emailService.SendAsync(
                new[]
                {
                    settings.CorreoSupervisor!
                },
                $"Nuevo Pedido N° {pedidoDto.Id}",
                html);
        }

        public async Task NotificarPedidoAprobadoAsync(int idPedido)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(idPedido);

            if (pedido == null)
                return;

            var pedidoDto = PedidoMapper.ToDto(pedido);

            var settings =
                await _configurationService.GetNotificationSettingsAsync();

            var html =
                await _templateService.PedidoAprobadoAsync(
                    pedidoDto,
                    settings);

            await _emailService.SendAsync(
                new[]
                {
                    settings.CorreoSupervisor!
                },
                $"Pedido N° {pedidoDto.Id} Aprobado",
                html);
        }

        public async Task NotificarSalidaAlmacenAsync(int idMovimiento)
        {
            // Lo implementaremos cuando hagamos la salida de almacén.
        }

        public async Task NotificarDevolucionAsync(int idMovimiento)
        {
            // Lo implementaremos cuando hagamos la devolución.
        }

        public async Task NotificarGarantiaCreadaAsync(int idGarantia)
        {
            var garantiaEntity = await _garantiaRepository.GetByIdAsync(idGarantia);

            if (garantiaEntity == null)
                throw new Exception("No se encontró la garantía.");

            // Entity -> DTO con detalles
            var garantia = _garantiaMapper.MapToDetailDto(garantiaEntity);

            var settings = await _configurationService.GetNotificationSettingsAsync();

            var html = await _templateService.GarantiaCreadaAsync(
                garantia,
                settings);

            await _emailService.SendAsync(
                new[]
                {
            settings.CorreoLogistica!
                },
                $"Nueva Garantía Registrada - {garantia.Id}",
                html);
        }

        public async Task NotificarGarantiaCerradaAsync(int idGarantia)
        {
            var garantiaEntity = await _garantiaRepository.GetByIdAsync(idGarantia);

            if (garantiaEntity == null)
                throw new Exception("No se encontró la garantía.");

            // Entity -> DTO con detalle
            var garantia = _garantiaMapper.MapToDetailDto(garantiaEntity);

            var settings = await _configurationService.GetNotificationSettingsAsync();

            var html = await _templateService.GarantiaCerradaAsync(
                garantia,
                settings);

            await _emailService.SendAsync(
                new[]
                {
            settings.CorreoLogistica!
                },
                $"Garantía Cerrada - {garantia.Id}",
                html);
        }
    }
}