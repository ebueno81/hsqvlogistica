using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsqvLogistica.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
        => Ok(await _service.GetSummaryAsync());

    [HttpGet("movements")]
    public async Task<IActionResult> GetMonthlyMovements()
        => Ok(await _service.GetMonthlyMovementsAsync());
}
