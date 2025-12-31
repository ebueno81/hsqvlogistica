using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Repositories.Interfaces
{
    public interface IMovimientoRepository
    {
        Task<MovimientoPagedResultDto> SearchAsync(
            MovimientoFilterDto filter,
            CancellationToken cancellationToken);

        Task<int> CreateAsync(Movimiento movimiento);
        Task<Movimiento?> GetByIdAsync(int id);
    }
}
