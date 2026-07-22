using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.DTOs.Pedidos;

namespace HsqvLogistica.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<string> PedidoCreadoAsync(PedidoDto pedido);

        Task<string> PedidoAprobadoAsync(PedidoDto pedido);

        Task<string> SalidaAlmacenAsync(MovimientoDto movimiento);

        Task<string> DevolucionAsync(MovimientoDto movimiento);
    }
}
