using HsqvLogistica.Models.DTOs.Almacen;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Mappers
{
    public class AlmacenMapper
    {
        public AlmacenDto MapToDto(Almacen almacen)
        {
            return new AlmacenDto
            {
                Id = almacen.Id,
                Nombre = almacen.Descripcion
            };
        }

        public IEnumerable<AlmacenDto> MapToDto(IEnumerable<Almacen> almacenes)
        {
            return almacenes.Select(MapToDto);
        }

        public Almacen MapToEntity(AlmacenDto dto)
        {
            return new Almacen
            {
                Id = dto.Id,
                Descripcion = dto.Nombre
            };
        }

        // Para actualizar una entidad existente
        public void MapToEntity(Almacen entity, AlmacenDto dto)
        {
            entity.Descripcion = dto.Nombre;
        }
    }
}