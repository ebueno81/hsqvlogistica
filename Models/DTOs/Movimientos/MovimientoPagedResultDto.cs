namespace HsqvLogistica.Models.DTOs.Movimientos
{
    public class MovimientoPagedResultDto
    {
        public List<MovimientoDto> Items { get; set; } = new();
        public int TotalItems { get; set; }
    }
}
