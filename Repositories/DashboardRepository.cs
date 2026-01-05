using HsqvLogistica.Data;
using HsqvLogistica.Models.DTOs.Dashboard;
using HsqvLogistica.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly StoreDbContext _context;

    public DashboardRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalArticulosAsync()
        => await _context.Articulos.CountAsync();

    public async Task<int> GetStockTotalAsync()
        => await _context.Articulos.SumAsync(a => a.Stock ?? 0);

    public async Task<int> GetPedidosActivosAsync()
        => await _context.Pedidos.CountAsync(p => p.Estado == 1);

    public async Task<int> GetGarantiasAsync()
        => await _context.Garantia.CountAsync(g => g.Activo == true);

    public async Task<List<MonthlyMovementDto>> GetMonthlyMovementsAsync()
    {
        // 🔹 Últimos 6 meses (incluye el mes actual)
        var today = DateOnly.FromDateTime(DateTime.Today);
        var from = today.AddMonths(-5);

        // 🔹 Query base (EF)
        var raw = await _context.Movimientos
            .Where(m => m.Fecha >= from && m.Fecha <= today)
            .GroupBy(m => new
            {
                m.Fecha!.Value.Year,
                m.Fecha!.Value.Month
            })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Total = g.Count()
            })
            .ToListAsync();

        // 🔹 Construimos SIEMPRE los 6 meses
        var result = new List<MonthlyMovementDto>();

        for (int i = 5; i >= 0; i--)
        {
            var date = today.AddMonths(-i);

            var monthData = raw.FirstOrDefault(x =>
                x.Year == date.Year &&
                x.Month == date.Month);

            result.Add(new MonthlyMovementDto
            {
                Mes = GetMonthName(date.Month),
                Total = monthData?.Total ?? 0
            });
        }

        return result;
    }

    private static string GetMonthName(int month) => month switch
    {
        1 => "Ene",
        2 => "Feb",
        3 => "Mar",
        4 => "Abr",
        5 => "May",
        6 => "Jun",
        7 => "Jul",
        8 => "Ago",
        9 => "Sep",
        10 => "Oct",
        11 => "Nov",
        12 => "Dic",
        _ => ""
    };
}
