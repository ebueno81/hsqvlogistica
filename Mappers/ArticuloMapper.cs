using HsqvLogistica.Models.DTOs.Articulos;
using HsqvLogistica.Models.Entities.Store;

public static class ArticuloMapper
{
    public static ArticuloDto ToDto(Articulo entity)
        => new()
        {
            Id = entity.Id,
            Codigo = entity.Codigo,
            IdLinea = entity.IdLinea,
            Descripcion = entity.Descripcion,
            Stock = entity.Stock,
            PrecioMn = entity.PrecioMn,
            PrecioUs = entity.PrecioUs,
            Activo = entity.Activo,
            Detalles = entity.Detalles
        };

    public static Articulo ToEntity(ArticuloCreateDto dto)
        => new()
        {
            Codigo = dto.Codigo,
            IdLinea = dto.IdLinea,
            Descripcion = dto.Descripcion,
            Stock = dto.Stock,
            PrecioMn = dto.PrecioMn,
            PrecioUs = dto.PrecioUs,
            Activo = true,
            FechaCreacion = DateTime.Now,
            Detalles = dto.Detalles
        };
}
