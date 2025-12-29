using HsqvLogistica.Models.DTOs.Lineas;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Mappers;

public static class LineaMapper
{
    public static LineaDto ToDto(Linea entity)
        => new()
        {
            Id = entity.Id,
            Descripcion = entity.Descripcion,
            Activo = entity.Activo
        };

    public static Linea ToEntity(LineaCreateDto dto)
        => new()
        {
            Descripcion = dto.Descripcion,
            Activo = true
        };
}
