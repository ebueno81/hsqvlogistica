namespace HsqvLogistica.Models.DTOs.Dashboard
{
    public class DashboardResumenDto
    {
        public int TotalArticulos { get; set; }
        public int StockTotal { get; set; }
        public int PedidosActivos { get; set; }
        public int Garantias { get; set; }
    }
}
