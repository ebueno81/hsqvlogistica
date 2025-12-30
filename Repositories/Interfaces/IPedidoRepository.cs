using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetAllAsync();
    Task<Pedido?> GetByIdAsync(int id);

    Task AddAsync(Pedido pedido);
    Task SaveChangesAsync();

    Task<bool> ChangeStatusAsync(int id, bool activo);
}
