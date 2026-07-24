using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetAllAsync();
    Task<Pedido?> GetByIdAsync(int id);

    Task AddAsync(Pedido pedido);
    Task SaveChangesAsync();
    Task<bool> AnularPedido(int id, string usuarioModifica);
    Task<bool> ChangeStatusAsync(int id, int activo, string usuarioModifica);
    Task<PedidoPagedResultDto> SearchAsync(PedidoFilterDto filter,
            CancellationToken cancellationToken);
    Task<List<DisponibilidadArticuloDto>> ObtenerDisponibilidadAsync(DateOnly fecha,
            IEnumerable<int> articulos, int? idPedidoExcluir = null);
}
