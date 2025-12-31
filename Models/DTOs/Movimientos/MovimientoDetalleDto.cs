namespace HsqvLogistica.Models.DTOs.Movimientos
{
    public class MovimientoDetalleDto
    {
        public int IdArticulo { get; set; }
        public string Articulo { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal? PrecioMn { get; set; }
        public decimal? PrecioUs { get; set; }
        public Guid RowKey { get; set; } = Guid.NewGuid();
    }
}
