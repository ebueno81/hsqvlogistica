namespace HsqvLogistica.Models.DTOs.Movimientos
{
    public class MovimientoCreateDto
    {
        public string Tipo { get; set; } = ""; // Entrada / Salida
        public int? IdPedido { get; set; }
        public int? IdCliente { get; set; }
        public int? IdAlmacen { get; set; }
        public string? Cliente { get; set; }
        public int IdMotivo { get; set; }
        public DateOnly? Fecha { get; set; }

        public string? SerieGuia { get; set; }
        public string? NroGuia { get; set; }
        public string? Detalles { get; set; }
        public string UsuaCreacion { get; set; } = "";

        public List<MovimientoDetalleDto> DetallesMovimiento { get; set; } = new();
    }
}
