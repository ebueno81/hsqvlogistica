using HsqvLogistica.Models.DTOs.Dashboard;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        return new DashboardSummaryDto
        {
            TotalArticulos = await _repository.GetTotalArticulosAsync(),
            StockTotal = await _repository.GetStockTotalAsync(),
            PedidosActivos = await _repository.GetPedidosActivosAsync(),
            Garantias = await _repository.GetGarantiasAsync(),

            // 🔹 Trends (luego se calculan correctamente)
            ArticulosTrend = 0,
            StockTrend = 0,
            PedidosTrend = 0,
            GarantiasTrend = 0
        };
    }

    public async Task<List<MonthlyMovementDto>> GetMonthlyMovementsAsync()
        => await _repository.GetMonthlyMovementsAsync();
}
