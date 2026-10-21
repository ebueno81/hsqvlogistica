using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly IDbContextFactory<StoreDbContext> _contextFactory;

    public PedidoRepository(
        IDbContextFactory<StoreDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<Pedido>> GetAllAsync()
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Pedidos
            .Include(p => p.PedidoDetalles)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Pedido?> GetByIdAsync(int id)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Pedidos
            .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.IdArticuloNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }



    public async Task AddAsync(Pedido pedido)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        // Agregar pedido con sus detalles
        await context.Pedidos.AddAsync(pedido);

        // Guardar en el mismo contexto
        int registros = await context.SaveChangesAsync();

        Console.WriteLine(
            $"PEDIDO REGISTRADO: {pedido.Id} - " +
            $"Entidades guardadas: {registros}");
    }

    public async Task<bool> ChangeStatusAsync(
    int id,
    int activo,
    string usuarioModifica)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var filas = await context.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC sp_ActualizarEstadoPedido
            @Id={id},
            @Id_Status={activo},
            @Usuario_Modifica={usuarioModifica}");

        return filas > 0;
    }

    public async Task<PedidoPagedResultDto> SearchAsync(
    PedidoFilterDto filter,
    CancellationToken cancellationToken)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync(cancellationToken);

        var query = context.Pedidos
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Cliente))
        {
            query = query.Where(p =>
                p.Cliente != null &&
                p.Cliente.Contains(filter.Cliente));
        }

        if (filter.FechaDesde.HasValue)
        {
            var desde = filter.FechaDesde.Value;

            query = query.Where(p =>
                p.FechaEntrega >= desde);
        }

        if (filter.FechaHasta.HasValue)
        {
            var hasta = filter.FechaHasta.Value;

            query = query.Where(p =>
                p.FechaEntrega <= hasta);
        }

        // FILTRO POR ESTADO DEL PEDIDO
        if (filter.Estado.HasValue)
        {
            query = query.Where(p =>
                p.Estado == filter.Estado.Value);
        }

        var total = await query.CountAsync(cancellationToken);

        // Garantizar valores válidos para la paginación
        int page = Math.Max(0, filter.Page);
        int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

        int skip = page * pageSize;

        var items = await query
            .OrderByDescending(p => p.Id)
            .Skip(skip)
            .Take(pageSize)
            .Select(p => new PedidoDto
            {
                Id = p.Id,
                Cliente = p.Cliente,
                FechaEntrega = p.FechaEntrega,
                Estado = p.Estado,
                UsuaCreacion = p.UsuaCreacion,
                Activo = p.Activo
            })
            .ToListAsync(cancellationToken);

        return new PedidoPagedResultDto
        {
            Items = items,
            TotalItems = total
        };
    }

    public async Task<bool> AnularPedido(
    int id,
    string usuarioModifica)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var filas = await context.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC sp_ActualizarAnularPedido
            @Id={id},
            @Usuario_Modifica={usuarioModifica}");

        return filas > 0;
    }

    public async Task<List<DisponibilidadArticuloDto>>
    ObtenerDisponibilidadAsync(
        DateOnly fecha,
        IEnumerable<int> articulos,
        int? idPedidoExcluir = null)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var idsArticulos = articulos.ToArray();

        var query = context.PedidoDetalles
            .AsNoTracking()
            .Where(d =>
                d.IdArticulo.HasValue &&
                idsArticulos.Contains(d.IdArticulo.Value) &&
                d.IdPedidoNavigation.Activo == true &&
                (d.IdPedidoNavigation.Estado == 0 ||
                 d.IdPedidoNavigation.Estado == 1) &&
                d.IdPedidoNavigation.FechaEntrega == fecha);

        if (idPedidoExcluir.HasValue)
        {
            query = query.Where(d =>
                d.IdPedido != idPedidoExcluir.Value);
        }

        return await query
            .GroupBy(d => d.IdArticulo)
            .Select(g => new DisponibilidadArticuloDto
            {
                IdArticulo = g.Key!.Value,
                CantidadReservada = (decimal)g.Sum(x => x.Cantidad)
            })
            .ToListAsync();
    }
}
