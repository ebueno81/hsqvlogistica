using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;
using System;

namespace HsqvLogistica.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepo;
    private readonly INotificationService _notificationService;

    public PedidoService(IPedidoRepository pedidoRepo, INotificationService notificationService)
    {
        _pedidoRepo = pedidoRepo;
        _notificationService = notificationService;
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

        try
        {
            await _notificationService.NotificarPedidoCreadoAsync(pedido.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
        return pedido.Id;
    }

    public async Task<bool> ChangeStatusAsync(int id, int estado, string usuarioModifica)
    {
        try
        {
            await _pedidoRepo.ChangeStatusAsync(id, estado, usuarioModifica);

            switch (estado)
            {
                case 1: // Aprobado
                    await _notificationService.NotificarPedidoAprobadoAsync(id);
                    break;

            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<PedidoPagedResultDto> SearchAsync(
    PedidoFilterDto filter,
    CancellationToken cancellationToken)
    {
        return await _pedidoRepo.SearchAsync(filter, cancellationToken);
    }

    public async Task<bool> AnularPedido(int id, string usuarioModifica)
        => await _pedidoRepo.AnularPedido(id, usuarioModifica);
}
