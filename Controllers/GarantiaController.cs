using HsqvLogistica.Models.DTOs.Garantias;
using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsqvLogistica.Controllers;

[ApiController]
[Route("api/garantias")]
public class GarantiaController : ControllerBase
{
    private readonly IGarantiaService _service;

    public GarantiaController(IGarantiaService service)
    {
        _service = service;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(GarantiaFilterDto filter)
        => Ok(await _service.SearchAsync(filter));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GarantiaCreateDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, GarantiaUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpPut("{id:int}/cerrar")]
    public async Task<IActionResult> Close(int id)
    {
        await _service.CloseAsync(id, "admin");
        return NoContent();
    }
}
