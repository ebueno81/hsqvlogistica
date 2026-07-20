using HsqvLogistica.Models.DTOs.Pedidos;

namespace HsqvLogistica.Services.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoDto>> GetAllAsync();
    Task<PedidoDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(PedidoCreateDto dto);
    Task<bool> ChangeStatusAsync(int id, int activo, string usuarioModifica);
    Task<bool> AnularPedido(int id, string usuarioModifica);
    Task<PedidoPagedResultDto> SearchAsync(PedidoFilterDto filter, CancellationToken cancellationToken);
}
