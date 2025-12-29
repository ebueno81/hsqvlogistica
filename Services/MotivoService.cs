using HsqvLogistica.Models.DTOs.Motivos;
using HsqvLogistica.Models.Mappers;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class MotivoService : IMotivoService
{
    private readonly IMotivoRepository _repository;

    public MotivoService(IMotivoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MotivoDto>> GetAllAsync()
        => (await _repository.GetAllAsync())
            .Select(MotivoMapper.ToDto);

    public async Task<MotivoDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : MotivoMapper.ToDto(entity);
    }

    public async Task<MotivoDto> CreateAsync(MotivoCreateDto dto)
    {
        var entity = MotivoMapper.ToEntity(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return MotivoMapper.ToDto(entity);
    }

    public async Task UpdateAsync(int id, MotivoUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Motivo no encontrado");

        entity.Descripcion = dto.Descripcion;
        entity.TipoMov = dto.TipoMov;

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task ToggleAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Motivo no encontrado");

        entity.Activo = !entity.Activo;
        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }
}
