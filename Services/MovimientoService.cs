using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _repository;

        public MovimientoService(IMovimientoRepository repository)
        {
            _repository = repository;
        }

        public Task<MovimientoPagedResultDto> SearchAsync(
            MovimientoFilterDto filter,
            CancellationToken cancellationToken)
            => _repository.SearchAsync(filter, cancellationToken);

        public async Task<int> CreateAsync(MovimientoCreateDto dto)
        {
            var movimiento = new Movimiento
            {
                IdPedido = dto.IdPedido,
                IdCliente = dto.IdCliente,
                Cliente = dto.Cliente,
                IdMotivo = dto.IdMotivo,
                Fecha = dto.Fecha,
                SerieGuia = dto.SerieGuia,
                NroGuia = dto.NroGuia,
                Detalles = dto.Detalles,
                Activo = true,
                UsuaCreacion = dto.UsuaCreacion,
                FechaCreacion = DateTime.Now,
                MovimientoDetalles = dto.DetallesMovimiento.Select(d => new MovimientoDetalle
                {
                    IdArticulo = d.IdArticulo,
                    Cantidad = d.Cantidad,
                    PrecioMn = d.PrecioMn,
                    PrecioUs = d.PrecioUs,
                    Activo = true
                }).ToList()
            };

            return await _repository.CreateAsync(movimiento);
        }

        public async Task<MovimientoDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return new MovimientoDto
            {
                Id = entity.Id,

                // Tipo se obtiene del motivo (ING / SAL)
                Tipo = entity.IdMotivoNavigation?.TipoMov,

                IdMotivo = entity.IdMotivo ?? 0,
                IdPedido = entity.IdPedido,
                Motivo = entity.IdMotivoNavigation?.Descripcion,

                IdCliente = entity.IdCliente,
                Cliente = entity.Cliente,

                Fecha = (DateOnly)entity.Fecha,

                SerieGuia = entity.SerieGuia,
                NroGuia = entity.NroGuia,

                Observacion = entity.Detalles,

                Detalles = entity.MovimientoDetalles.Select(d => new MovimientoDetalleDto
                {
                    IdArticulo = d.IdArticulo ?? 0,
                    Articulo = d.IdArticuloNavigation?.Descripcion ?? "", // ✅ SEGURO
                    Cantidad = d.Cantidad ?? 0
                }).ToList()
            };
        }
    }

}
