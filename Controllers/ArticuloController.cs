using HsqvLogistica.Models.DTOs.Articulos;
using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/articulos")]
public class ArticuloController : ControllerBase
{
    private readonly IArticuloService _service;

    public ArticuloController(IArticuloService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ArticuloCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] ArticuloUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> PatchStatus(int id, [FromBody] ArticuloStatusDto dto)
    {
        var updated = await _service.ChangeStatusAsync(id, dto.Activo, dto.UsuarioModificacion);
        return updated ? NoContent() : NotFound();
    }
}
