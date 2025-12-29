using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Usuarios;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        => (await _repository.GetAllAsync())
            .Select(UsuarioMapper.ToDto);

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : UsuarioMapper.ToDto(entity);
    }

    public async Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto)
    {
        var entity = UsuarioMapper.ToEntity(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return UsuarioMapper.ToDto(entity);
    }

    public async Task UpdateAsync(int id, UsuarioUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado");

        entity.Nombres = dto.Nombres;
        entity.Correo = dto.Correo;
        entity.IdTipo = dto.IdTipo;

        if (!string.IsNullOrWhiteSpace(dto.Clave))
            entity.Clave = dto.Clave;

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id, bool activo)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
            return false;

        usuario.Activo = activo;
        await _repository.UpdateAsync(usuario);
        await _repository.SaveChangesAsync();

        return true;
    }
}
