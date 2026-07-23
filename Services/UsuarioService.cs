using DocumentFormat.OpenXml.Office2010.Excel;
using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Usuarios;
using HsqvLogistica.Repositories;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly INotificationService _notificationService;
    public UsuarioService(IUsuarioRepository repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
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

        await _notificationService.NotificarUsuarioCreadoAsync(entity.Id);

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

        await _notificationService.NotificarUsuarioActualizadoAsync(id);
    }

    public async Task<bool> ChangeStatusAsync(int id, bool activo)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
            return false;

        usuario.Activo = activo;
        await _repository.UpdateAsync(usuario);
        await _repository.SaveChangesAsync();

        if (activo)
            await _notificationService.NotificarUsuarioActualizadoAsync(id);

        return true;
    }

    public async Task<UsuarioDto?> ValidateUserAsync(string user, string pass)
    {
        var usuario = await _repository.ValidateUserAsync(user, pass);

        return usuario == null
            ? null
            : UsuarioMapper.ToDto(usuario);
    }

    public async Task<UsuarioDto?> GetByUsuarioAsync(string usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario))
            return null;

        return await _repository.GetByUsuarioAsync(usuario.Trim());
    }
}
