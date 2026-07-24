using HsqvLogistica.Common.Exceptions;
using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly INotificationService _notificationService;
    private readonly IArticuloRepository _articuloRepository;

    public PedidoService(IPedidoRepository pedidoRepo, INotificationService notificationService, IArticuloRepository articuloRepository)
    {
        _pedidoRepository = pedidoRepo;
        _notificationService = notificationService;
        _articuloRepository = articuloRepository;
    }

    public async Task<IEnumerable<PedidoDto>> GetAllAsync()
        => (await _pedidoRepository.GetAllAsync())
            .Select(PedidoMapper.ToDto);

    public async Task<PedidoDto?> GetByIdAsync(int id)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(id);
        return pedido == null ? null : PedidoMapper.ToDto(pedido);
    }

    public async Task<int> CreateAsync(PedidoCreateDto dto)
    {
        await ValidarDisponibilidadAsync(dto);

        var pedido = PedidoMapper.ToEntity(dto);

        foreach (var det in dto.DetallesPedido)
        {
            pedido.PedidoDetalles.Add(new PedidoDetalle
            {
                IdArticulo = det.IdArticulo,
                Cantidad = det.Cantidad,
                Activo = true
            });
        }

        await _pedidoRepository.AddAsync(pedido);

        await _pedidoRepository.SaveChangesAsync();

        try
        {
            await _notificationService.NotificarPedidoCreadoAsync(pedido.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return pedido.Id;
    }

    public async Task<bool> ChangeStatusAsync(int id, int estado, string usuarioModifica)
    {
        try
        {
            await _pedidoRepository.ChangeStatusAsync(id, estado, usuarioModifica);

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
        return await _pedidoRepository.SearchAsync(filter, cancellationToken);
    }

    public async Task<bool> AnularPedido(int id, string usuarioModifica)
        => await _pedidoRepository.AnularPedido(id, usuarioModifica);

    private async Task ValidarDisponibilidadAsync(PedidoCreateDto pedido,
        int? idPedidoExcluir = null)
    {
        // Artículos distintos del pedido
        var idsArticulos = pedido.DetallesPedido
            .Where(x => x.IdArticulo.HasValue)
            .Select(x => x.IdArticulo!.Value)
            .Distinct()
            .ToList();

        if (!idsArticulos.Any())
            return;

        // Cantidad ya reservada para la fecha
        var reservas = await _pedidoRepository.ObtenerDisponibilidadAsync(
            pedido.FechaEntrega,
            idsArticulos,
            idPedidoExcluir);

        // Obtener información de los artículos
        var articulos = await _articuloRepository.GetByIdsAsync(idsArticulos);

        var errores = new List<string>();

        foreach (var detalle in pedido.DetallesPedido)
        {
            if (!detalle.IdArticulo.HasValue)
                continue;

            var articulo = articulos.First(x => x.Id == detalle.IdArticulo.Value);

            var reservado = reservas
                .FirstOrDefault(x => x.IdArticulo == detalle.IdArticulo.Value)
                ?.CantidadReservada ?? 0;

            var disponible = articulo.StockFijo - reservado;

            if (detalle.Cantidad > disponible)
            {
                errores.Add(
                    $"{articulo.Codigo} - {articulo.Descripcion}\n" +
                    $"Disponible: {disponible}\n" +
                    $"Solicitado: {detalle.Cantidad}");
            }
        }

        if (errores.Any())
        {
            throw new BusinessException(
                "No se puede completar la operación.\n\n" +
                "Los siguientes artículos no tienen disponibilidad para la fecha seleccionada:\n\n" +
                string.Join("\n\n", errores));
        }
    }
}
