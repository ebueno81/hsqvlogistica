
using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly IDbContextFactory<StoreDbContext> _contextFactory;

        public MovimientoRepository(
            IDbContextFactory<StoreDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // ==========================================
        // LISTADO DE MOVIMIENTOS - KARDEX
        // ==========================================
        public async Task<MovimientoPagedResultDto> SearchAsync(
            MovimientoFilterDto filter,
            CancellationToken cancellationToken)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync(cancellationToken);

            var query = context.Movimientos
                .AsNoTracking()
                .AsQueryable();

            // Filtro por tipo de movimiento
            if (!string.IsNullOrEmpty(filter.Tipo))
            {
                query = query.Where(x =>
                    x.IdMotivoNavigation!.TipoMov == filter.Tipo);
            }

            // Filtro por cliente
            if (!string.IsNullOrEmpty(filter.Cliente))
            {
                query = query.Where(x =>
                    x.Cliente != null &&
                    x.Cliente.Contains(filter.Cliente));
            }

            // Fecha desde
            if (filter.FechaDesde.HasValue)
            {
                query = query.Where(x =>
                    x.Fecha >= filter.FechaDesde);
            }

            // Fecha hasta
            if (filter.FechaHasta.HasValue)
            {
                query = query.Where(x =>
                    x.Fecha <= filter.FechaHasta);
            }

            var total = await query.CountAsync(cancellationToken);

            // Evitar OFFSET negativo
            int page = Math.Max(0, filter.Page);
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            int skip = page * pageSize;

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
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

        // ==========================================
        // REGISTRAR MOVIMIENTO Y ACTUALIZAR STOCK
        // ==========================================
        public async Task<int> CreateAsync(Movimiento movimiento)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            await using var transaction =
                await context.Database.BeginTransactionAsync();

            try
            {
                // Registrar cabecera y detalles
                context.Movimientos.Add(movimiento);

                await context.SaveChangesAsync();

                // Actualizar stock dentro de la misma transacción
                await ActualizarStockAsync(
                    context,
                    movimiento.Id,
                    1);

                await transaction.CommitAsync();

                return movimiento.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // ==========================================
        // OBTENER MOVIMIENTO POR ID
        // ==========================================
        public async Task<Movimiento?> GetByIdAsync(int id)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.Movimientos
                .AsNoTracking()
                .Include(m => m.IdMotivoNavigation)
                .Include(m => m.MovimientoDetalles)
                    .ThenInclude(d => d.IdArticuloNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // ==========================================
        // ANULAR MOVIMIENTO Y REVERTIR STOCK
        // ==========================================
        public async Task<bool> AnularMovimiento(
            int id,
            string usuarioModifica)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            await using var transaction =
                await context.Database.BeginTransactionAsync();

            try
            {
                // Anular movimiento
                await context.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC sp_ActualizarAnularMovimiento
                        @Id = {id},
                        @Usuario_Modifica = {usuarioModifica}");

                // Revertir stock en la misma transacción
                await ActualizarStockAsync(
                    context,
                    id,
                    -1);

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // ==========================================
        // ACTUALIZAR STOCK
        // ==========================================
        private static async Task ActualizarStockAsync(
            StoreDbContext context,
            int idMovimiento,
            int factor)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC usp_ActualizarStockArticulo
                    @IdMovimiento = {idMovimiento},
                    @Factor = {factor}");
        }
    }
}
