using HsqvLogistica.Models.DTOs.Pedidos;

namespace HsqvLogistica.Services.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoDto>> GetAllAsync();
    Task<PedidoDto?> GetByIdAsync(int id);

    Task<int> CreateAsync(PedidoCreateDto dto);

    Task<bool> ChangeStatusAsync(int id, bool activo);
}
