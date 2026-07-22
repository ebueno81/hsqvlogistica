using HsqvLogistica.Mappers;
using HsqvLogistica.Models.DTOs.Garantia;
using HsqvLogistica.Models.DTOs.Garantias;
using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class GarantiaService : IGarantiaService
{
    private readonly IGarantiaRepository _repository;
    private readonly INotificationService _notificationService;
    private readonly GarantiaMapper _mapper;

    public GarantiaService(
        IGarantiaRepository repository,
        GarantiaMapper mapper,
        INotificationService notificationService)
    {
        _repository = repository;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<GarantiaPagedResultDto> SearchAsync(GarantiaFilterDto filter)
    {
        var (items, total) = await _repository.SearchAsync(
            filter.Cliente,
            filter.Estado,
            filter.FechaDesde,
            filter.FechaHasta,
            filter.Page,
            filter.PageSize);

        return new GarantiaPagedResultDto
        {
            Items = items.Select(_mapper.MapToDto).ToList(),
            TotalItems = total
        };
    }

    public async Task<GarantiaDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return null;

        return _mapper.MapToDetailDto(entity);
    }

    public async Task<int> CreateAsync(GarantiaCreateDto dto)
    {
        var entity = new Garantium
        {
            IdCliente = dto.IdCliente,
            IdEmpServ = dto.IdEmpServ,
            Cliente = dto.Cliente,
            EmpServ = dto.EmpServ,
            FechaDespacho = dto.FechaDespacho,
            FechaEntrega = dto.FechaEntrega,
            NroSerie = dto.NroSerie,
            NroGuia = dto.NroGuia,
            Detalles = dto.Detalles,
            Estado = false, // 🔴 PENDIENTE
            Activo = true,
            UsuaCreacion = dto.UsuaCreacion,
            FechaCreacion = DateTime.Now,
            GarantiaDetalles = dto.GarantiaDetalles.Select(d => new GarantiaDetalle
            {
                IdArticulo = d.IdArticulo,
               Cantidad = d.Cantidad,
               Detalles = d.Detalles,
                Activo = true
            }).ToList()
        };

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        try
        {
            await _notificationService.NotificarGarantiaCreadaAsync(entity.Id);
        }
        catch
        {
            // No impedir el registro si falla el correo
        }
        return entity.Id;
    }

    public async Task UpdateAsync(int id, GarantiaUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Garantía no encontrada");

        if (entity.Estado)
            throw new Exception("La garantía está cerrada");

        entity.FechaDespacho = dto.FechaDespacho;
        entity.FechaEntrega = dto.FechaEntrega;
        entity.Detalles = dto.Detalles;
        entity.UsuaModifica = dto.UsuaModifica;
        entity.FechaModifica = DateTime.Now;
        entity.NroSerie = dto.NroSerie;
        entity.NroGuia = dto.NroGuia;

        await _repository.SaveChangesAsync();
    }

    public async Task CloseAsync(int id, string usuario)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Garantía no encontrada");

        entity.Estado = true;
        entity.UsuaModifica = usuario;
        entity.FechaModifica = DateTime.Now;

        await _repository.SaveChangesAsync();
        try
        {
            await _notificationService.NotificarGarantiaCerradaAsync(id);
        }
        catch
        {
            // No impedir el registro si falla el correo
        }
    }

    public async Task AnularAsync(int id, string usuario)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        if (!entity.Estado) return;

        entity.Activo = false;
        entity.UsuaModifica = usuario;
        entity.FechaModifica = DateTime.Now;

        await _repository.UpdateAsync(entity);
    }

    public async Task ActivarAsync(int id, string usuario)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        entity.Activo = true;
        entity.UsuaModifica = usuario;
        entity.FechaModifica = DateTime.Now;

        await _repository.UpdateAsync(entity);
    }

}
