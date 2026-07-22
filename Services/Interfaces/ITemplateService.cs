using HsqvLogistica.Common.Configuration;
using HsqvLogistica.Models.DTOs.Garantias;
using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.DTOs.Pedidos;

namespace HsqvLogistica.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<string> PedidoCreadoAsync(
            PedidoDto pedido,
            NotificationSettings settings);

        Task<string> PedidoAprobadoAsync(
            PedidoDto pedido,
            NotificationSettings settings);

        Task<string> SalidaAlmacenAsync(
            MovimientoDto movimiento,
            NotificationSettings settings);

        Task<string> DevolucionAsync(
            MovimientoDto movimiento,
            NotificationSettings settings);

        Task<string> GarantiaCreadaAsync(
            GarantiaDto garantia,
            NotificationSettings settings);

        Task<string> GarantiaCerradaAsync(
            GarantiaDto garantia,
            NotificationSettings settings);
    }
}
