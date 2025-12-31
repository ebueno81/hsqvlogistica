namespace HsqvLogistica.Models.DTOs.Movimientos
{
    public class MovimientoFilterDto
    {
        public string? Tipo { get; set; }
        public string? Cliente { get; set; }
        public DateOnly? FechaDesde { get; set; }
        public DateOnly? FechaHasta { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
