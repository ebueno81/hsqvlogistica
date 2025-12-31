using HsqvLogistica.Models.DTOs.Movimientos;

namespace HsqvLogistica.Services.Interfaces
{
    public interface IMovimientoService
    {
        Task<MovimientoPagedResultDto> SearchAsync(
            MovimientoFilterDto filter,
            CancellationToken cancellationToken);

        Task<int> CreateAsync(MovimientoCreateDto dto);
        Task<MovimientoDto?> GetByIdAsync(int id);
    }
}
