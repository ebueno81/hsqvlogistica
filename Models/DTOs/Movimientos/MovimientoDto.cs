namespace HsqvLogistica.Models.DTOs.Movimientos
{
    public class MovimientoDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = default!;
        public string Motivo { get; set; } = default!;
        public int IdMotivo { get; set; }
        public int? IdPedido { get; set; }

        public int? IdCliente { get; set; }
        public string? Cliente { get; set; }

        public DateOnly Fecha { get; set; }

        public string? SerieGuia { get; set; }
        public string? NroGuia { get; set; }
        public string? Observacion { get; set; }
        public bool? Activo { get; set; }

        public List<MovimientoDetalleDto> Detalles { get; set; } = new();
    }
}
