using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly StoreDbContext _context;

        public MovimientoRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<MovimientoPagedResultDto> SearchAsync(
            MovimientoFilterDto filter,
            CancellationToken cancellationToken)
        {
            var query = _context.Movimientos.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Tipo))
                query = query.Where(x => x.IdMotivoNavigation!.TipoMov == filter.Tipo);

            if (!string.IsNullOrEmpty(filter.Cliente))
                query = query.Where(x => x.Cliente!.Contains(filter.Cliente));

            if (filter.FechaDesde.HasValue)
                query = query.Where(x => x.Fecha >= filter.FechaDesde);

            if (filter.FechaHasta.HasValue)
                query = query.Where(x => x.Fecha <= filter.FechaHasta);

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip(filter.Page * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new MovimientoDto
                {
                    Id = x.Id,
                    IdPedido = x.IdPedido,
                    IdMotivo = x.IdMotivo ?? 0,
                    Cliente = x.Cliente,
                    Fecha = (DateOnly)x.Fecha,
                    SerieGuia = x.SerieGuia,
                    NroGuia = x.NroGuia,
                    Activo = x.Activo,
                    Motivo = x.IdMotivoNavigation!.Descripcion,
                    Tipo = x.IdMotivoNavigation!.TipoMov
                })
                .ToListAsync(cancellationToken);

            return new MovimientoPagedResultDto
            {
                Items = items,
                TotalItems = total
            };
        }

        public async Task<int> CreateAsync(Movimiento movimiento)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Movimientos.Add(movimiento);

                await _context.SaveChangesAsync();

                // Actualiza stock
                await ActualizarStockAsync(movimiento.Id, 1);

                await transaction.CommitAsync();

                return movimiento.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Movimiento?> GetByIdAsync(int id)
        {
            return await _context.Movimientos
                .Include(m => m.IdMotivoNavigation)
                .Include(m => m.MovimientoDetalles)
                    .ThenInclude(d => d.IdArticuloNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> AnularMovimiento(int id, string usuarioModifica)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var filas = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC sp_ActualizarAnularMovimiento
                @Id = {id},
                @Usuario_Modifica = {usuarioModifica}");

                await ActualizarStockAsync(id, -1);
               
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task ActualizarStockAsync(int idMovimiento, int factor)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC usp_ActualizarStockArticulo
                @IdMovimiento = {idMovimiento},
                @Factor = {factor}");
        }
    }

}
