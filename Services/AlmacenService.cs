using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class AlmacenService : IAlmacenService
{
    private readonly IAlmacenRepository _repository;

    public AlmacenService(IAlmacenRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Almacen>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Almacen?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<Almacen> CreateAsync(Almacen entity)
    {
        // 🧠 regla de negocio ejemplo
        entity.Activo ??= true;

        await _repository.AddAsync(entity);
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Almacen entity)
    {
        var current = await _repository.GetByIdAsync(id);
        if (current == null) return false;

        current.Descripcion = entity.Descripcion;
        current.Activo = entity.Activo;

        await _repository.UpdateAsync(current);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return false;

        // 🧠 regla de negocio
        if (entity.Activo == true)
            throw new InvalidOperationException("No se puede eliminar un almacén activo.");

        await _repository.DeleteAsync(entity);
        return true;
    }
}
