using HsqvLogistica.Models.DTOs.Articulos;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

public class ArticuloService : IArticuloService
{
    private readonly IArticuloRepository _repository;

    public ArticuloService(IArticuloRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ArticuloDto>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ArticuloDto?> GetByIdAsync(int id)
    {
        var articulo = await _repository.GetByIdAsync(id);
        return articulo == null ? null : ArticuloMapper.ToDto(articulo);
    }

    public async Task<ArticuloDto> CreateAsync(ArticuloCreateDto dto)
    {
        dto.Codigo = dto.Codigo.Trim().ToUpperInvariant();

        var entity = ArticuloMapper.ToEntity(dto);

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return ArticuloMapper.ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, ArticuloUpdateDto dto)
    {
        var articulo = await _repository.GetByIdAsync(id);
        if (articulo == null) return false;

        articulo.Descripcion = dto.Descripcion;
        articulo.Stock = dto.Stock;
        articulo.PrecioMn = dto.PrecioMn;
        articulo.PrecioUs = dto.PrecioUs;
        articulo.FechaModifica = DateTime.Now;
        articulo.RutaImagen = dto.RutaImagen;
        articulo.IdLinea = dto.IdLinea;
        articulo.Detalles = dto.Detalles;
        articulo.UsuaModifica = dto.UsuaModifica;

        await _repository.UpdateAsync(articulo);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangeStatusAsync(int id, bool activo, string usuarioModifica)
    {
        var articulo = await _repository.GetByIdAsync(id);
        if (articulo == null) return false;

        articulo.Activo = activo;
        articulo.FechaModifica = DateTime.Now;
        articulo.UsuaModifica = usuarioModifica;
        await _repository.UpdateAsync(articulo);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ArticuloLookupDto>> SearchLookupAsync(string filtro)
    {
        return (await _repository.SearchAsync(filtro))
            .Select(a => new ArticuloLookupDto
            {
                Id = a.Id,
                Descripcion = a.Descripcion
            });
    }

}
