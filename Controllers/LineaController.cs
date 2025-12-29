using HsqvLogistica.Models.DTOs.Lineas;
using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsqvLogistica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LineaController : ControllerBase
{
    private readonly ILineaService _service;

    public LineaController(ILineaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var linea = await _service.GetByIdAsync(id);
        return linea is null ? NotFound() : Ok(linea);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LineaCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] LineaUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);

        return updated ? NoContent() : NotFound();
    }

    // 🔥 PATCH lógico (activar / desactivar)
    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> PatchEstado(int id, [FromQuery] bool activo)
    {
        var result = await _service.ChangeStatusAsync(id, activo);
        return result ? NoContent() : NotFound();
    }
}
