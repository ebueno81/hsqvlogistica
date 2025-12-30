using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepo;

    public PedidoService(IPedidoRepository pedidoRepo)
    {
        _pedidoRepo = pedidoRepo;
    }

    public async Task<IEnumerable<PedidoDto>> GetAllAsync()
        => (await _pedidoRepo.GetAllAsync())
            .Select(PedidoMapper.ToDto);

    public async Task<PedidoDto?> GetByIdAsync(int id)
    {
        var pedido = await _pedidoRepo.GetByIdAsync(id);
        return pedido == null ? null : PedidoMapper.ToDto(pedido);
    }

    public async Task<int> CreateAsync(PedidoCreateDto dto)
    {
        var pedido = PedidoMapper.ToEntity(dto);

        foreach (var det in dto.DetallesPedido)
        {
            pedido.PedidoDetalles.Add(new PedidoDetalle
            {
                IdArticulo = det.IdArticulo,
                Cantidad = det.Cantidad
            });
        }

        await _pedidoRepo.AddAsync(pedido);
        await _pedidoRepo.SaveChangesAsync();

        return pedido.Id;
    }

    public async Task<bool> ChangeStatusAsync(int id, bool activo)
        => await _pedidoRepo.ChangeStatusAsync(id, activo);
}
