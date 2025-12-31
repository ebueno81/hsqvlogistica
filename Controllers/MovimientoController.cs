using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsqvLogistica.Controllers
{
    [ApiController]
    [Route("api/movimientos")]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoService _service;

        public MovimientoController(IMovimientoService service)
        {
            _service = service;
        }

        [HttpPost("search")]
        public Task<MovimientoPagedResultDto> Search(
            [FromBody] MovimientoFilterDto filter,
            CancellationToken cancellationToken)
            => _service.SearchAsync(filter, cancellationToken);

        [HttpPost]
        public Task<int> Create([FromBody] MovimientoCreateDto dto)
            => _service.CreateAsync(dto);
    }
}
