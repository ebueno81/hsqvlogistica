using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsqvLogistica.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidoController(IPedidoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pedido = await _service.GetByIdAsync(id);
        return pedido == null ? NotFound() : Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PedidoCreateDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return Ok(new { id });
    }

    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> ChangeStatus(int id, [FromQuery] bool activo)
    {
        var ok = await _service.ChangeStatusAsync(id, activo);
        return ok ? Ok() : NotFound();
    }
}
