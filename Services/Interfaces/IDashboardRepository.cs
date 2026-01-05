using HsqvLogistica.Models.DTOs.Dashboard;

namespace HsqvLogistica.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
    Task<List<MonthlyMovementDto>> GetMonthlyMovementsAsync();
}
