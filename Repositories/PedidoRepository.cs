using HsqvLogistica.Data;
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
}
