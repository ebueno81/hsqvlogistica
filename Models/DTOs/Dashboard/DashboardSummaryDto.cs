namespace HsqvLogistica.Models.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalArticulos { get; set; }
    public int StockTotal { get; set; }
    public int PedidosActivos { get; set; }
    public int Garantias { get; set; }

    public decimal ArticulosTrend { get; set; }
    public decimal StockTrend { get; set; }
    public decimal PedidosTrend { get; set; }
    public decimal GarantiasTrend { get; set; }
}
