using HsqvLogistica.Models.DTOs.Motivos;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Models.Mappers;

public static class MotivoMapper
{
    public static MotivoDto ToDto(Motivo entity)
        => new()
        {
            Id = entity.Id,
            Descripcion = entity.Descripcion,
            TipoMov = entity.TipoMov,
            Activo = entity.Activo
        };

    public static Motivo ToEntity(MotivoCreateDto dto)
        => new()
        {
            Descripcion = dto.Descripcion,
            TipoMov = dto.TipoMov,
            Activo = true
        };
}
