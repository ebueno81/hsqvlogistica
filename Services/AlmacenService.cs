using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Almacen;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class AlmacenService : IAlmacenService
{
    private readonly IAlmacenRepository _repository;
    private readonly AlmacenMapper _mapper;

    public AlmacenService(
        IAlmacenRepository repository,
        AlmacenMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<AlmacenDto>> GetAllAsync()
    {
        var almacenes = await _repository.GetAllAsync();

        return _mapper
            .MapToDto(almacenes)
            .ToList();
    }

    public async Task<AlmacenDto?> GetByIdAsync(int id)
    {
        var almacen = await _repository.GetByIdAsync(id);

        if (almacen == null)
            return null;

        return _mapper.MapToDto(almacen);
    }

    public async Task<AlmacenDto> CreateAsync(AlmacenDto dto)
    {
        var entity = _mapper.MapToEntity(dto);

        entity.Activo ??= true;

        await _repository.AddAsync(entity);

        return _mapper.MapToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, AlmacenDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        _mapper.MapToEntity(entity, dto);

        await _repository.UpdateAsync(entity);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        if (entity.Activo == true)
            throw new InvalidOperationException(
                "No se puede eliminar un almacén activo.");

        await _repository.DeleteAsync(entity);

        return true;
    }


}