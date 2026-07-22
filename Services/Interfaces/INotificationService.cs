namespace HsqvLogistica.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotificarPedidoCreadoAsync(int idPedido);

        Task NotificarPedidoAprobadoAsync(int idPedido);

        Task NotificarSalidaAlmacenAsync(int idMovimiento);

        Task NotificarDevolucionAsync(int idMovimiento);

        Task NotificarGarantiaCreadaAsync(int idGarantia);

        Task NotificarGarantiaCerradaAsync(int idGarantia);

    }
}