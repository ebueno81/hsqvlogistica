using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Lineas;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class LineaService : ILineaService
{
    private readonly ILineaRepository _repository;

    public LineaService(ILineaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LineaDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();
        return data.Select(LineaMapper.ToDto);
    }

    public async Task<LineaDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : LineaMapper.ToDto(entity);
    }

    public async Task<LineaDto> CreateAsync(LineaCreateDto dto)
    {
        var entity = LineaMapper.ToEntity(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return LineaMapper.ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, LineaUpdateDto dto)
    {
        var linea = await _repository.GetByIdAsync(id);
        if (linea is null) return false;

        linea.Descripcion = dto.Descripcion;

        await _repository.UpdateAsync(linea);
        return true;
    }

    public async Task<bool> ChangeStatusAsync(int id, bool activo)
    {
        var linea = await _repository.GetByIdAsync(id);
        if (linea is null) return false;

        linea.Activo = activo;

        await _repository.UpdateAsync(linea);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LineaDto>> GetActivasAsync()
    {
        var data = await _repository.GetAllAsync();
        return data
            .Where(x => x.Activo == true)
            .Select(LineaMapper.ToDto);
    }

}
