using HsqvLogistica.Models.Entities.Store;
using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsqvLogistica.Controllers;

[ApiController]
[Route("api/almacen")]
public class AlmacenController : ControllerBase
{
    private readonly IAlmacenService _service;

    public AlmacenController(IAlmacenService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Almacen model)
        => Ok(await _service.CreateAsync(model));

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Almacen model)
        => await _service.UpdateAsync(id, model)
            ? NoContent()
            : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.DeleteAsync(id)
            ? NoContent()
            : NotFound();
}
