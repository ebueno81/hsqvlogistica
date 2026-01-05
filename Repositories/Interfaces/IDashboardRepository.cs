using HsqvLogistica.Models.DTOs.Dashboard;

namespace HsqvLogistica.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<int> GetTotalArticulosAsync();
    Task<int> GetStockTotalAsync();
    Task<int> GetPedidosActivosAsync();
    Task<int> GetGarantiasAsync();

    Task<List<MonthlyMovementDto>> GetMonthlyMovementsAsync();
}
