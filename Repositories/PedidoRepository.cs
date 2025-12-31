using HsqvLogistica.Data;
using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly StoreDbContext _context;

    public PedidoRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> GetAllAsync() =>
        await _context.Pedidos
            .Include(p => p.PedidoDetalles)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Pedido?> GetByIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.IdArticuloNavigation) 
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Pedido pedido)
        => await _context.Pedidos.AddAsync(pedido);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<bool> ChangeStatusAsync(int id, bool activo)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null) return false;

        pedido.Activo = activo;
        pedido.FechaModifica = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PedidoPagedResultDto> SearchAsync(
    PedidoFilterDto filter,
    CancellationToken cancellationToken)
    {
        var query = _context.Pedidos.AsQueryable();

        // 🔎 Filtro por cliente
        if (!string.IsNullOrWhiteSpace(filter.Cliente))
        {
            query = query.Where(p =>
                p.Cliente != null &&
                p.Cliente.Contains(filter.Cliente));
        }

        // 📅 Filtro fecha desde
        if (filter.FechaDesde.HasValue)
        {
            var desde = filter.FechaDesde.Value;
            query = query.Where(p => p.FechaEntrega >= desde);
        }

        // 📅 Filtro fecha hasta
        if (filter.FechaHasta.HasValue)
        {
            var hasta = filter.FechaHasta.Value;
            query = query.Where(p => p.FechaEntrega <= hasta);
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.Id)
            .Skip(filter.Page * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => new PedidoDto
            {
                Id = p.Id,
                Cliente = p.Cliente,
                FechaEntrega = p.FechaEntrega,
                Estado = p.Estado,
                Activo = p.Activo
            })
            .ToListAsync(cancellationToken);

        return new PedidoPagedResultDto
        {
            Items = items,
            TotalItems = total
        };
    }

}
