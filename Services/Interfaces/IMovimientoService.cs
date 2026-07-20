using HsqvLogistica.Models.DTOs.Movimientos;

namespace HsqvLogistica.Services.Interfaces
{
    public interface IMovimientoService
    {
        Task<MovimientoPagedResultDto> SearchAsync(
            MovimientoFilterDto filter,
            CancellationToken cancellationToken);

        Task<int> CreateAsync(MovimientoDto dto);
        Task<bool> AnularMovimiento(int id, string usuarioModifica);
        Task<MovimientoDto?> GetByIdAsync(int id);
    }
}
