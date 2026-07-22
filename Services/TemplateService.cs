using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services
{
    public class TemplateService : ITemplateService
    {
        public Task<string> PedidoCreadoAsync(PedidoDto pedido)
        {
            var html = $@"
                <h2>Nuevo Pedido</h2>
                <p>Se registró el pedido <b>{pedido.Id}</b>.</p>";

            return Task.FromResult(html);
        }

        public Task<string> PedidoAprobadoAsync(PedidoDto pedido)
            => Task.FromResult(string.Empty);

        public Task<string> SalidaAlmacenAsync(MovimientoDto movimiento)
            => Task.FromResult(string.Empty);

        public Task<string> DevolucionAsync(MovimientoDto movimiento)
            => Task.FromResult(string.Empty);
    }
}