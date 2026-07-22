using HsqvLogistica.Mappers;
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

        public NotificationService(
            IPedidoRepository pedidoRepository,
            IMovimientoRepository movimientoRepository,
            IConfigurationService configurationService,
            ITemplateService templateService,
            IEmailService emailService)
        {
            _pedidoRepository = pedidoRepository;
            _movimientoRepository = movimientoRepository;
            _configurationService = configurationService;
            _templateService = templateService;
            _emailService = emailService;
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
    }
}