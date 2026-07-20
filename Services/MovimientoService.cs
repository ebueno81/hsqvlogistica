using HsqvLogistica.Mappers;
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

        public async Task<int> CreateAsync(MovimientoDto dto)
        {
            var movimiento = MovimientoMapper.ToEntity(dto);

            return await _repository.CreateAsync(movimiento);
        }

        public async Task<bool> AnularMovimiento(int id, string usuarioModifica)
        => await _repository.AnularMovimiento(id, usuarioModifica);

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
