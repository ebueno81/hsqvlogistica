using HsqvLogistica.Models.DTOs.Usuarios;
using HsqvLogistica.Models.Entities.Store;

namespace HsqvLogistica.Mappers;

public static class UsuarioMapper
{
    public static UsuarioDto ToDto(Usuario entity) => new()
    {
        Id = entity.Id,
        Usuario = entity.Usuario1,
        Nombres = entity.Nombres,
        Correo = entity.Correo,
        IdTipo = entity.IdTipo ?? 0,
        TipoUsuario = entity.IdTipoNavigation?.Descripcion,
        Activo = (bool)entity.Activo
    };

    public static Usuario ToEntity(UsuarioCreateDto dto) => new()
    {
        Usuario1 = dto.Usuario,
        Nombres = dto.Nombres,
        Correo = dto.Correo,
        Clave = dto.Clave,
        IdTipo = dto.IdTipo,
        Activo=true
    };
}
