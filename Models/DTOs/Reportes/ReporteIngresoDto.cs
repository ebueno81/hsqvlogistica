namespace HsqvLogistica.Models.DTOs.Reportes
{
    public class ReporteIngresoDto
    {
        public string Codigo { get; set; } = "";

        public string Linea { get; set; } = "";

        public string Articulo { get; set; } = "";

        public DateTime Fecha { get; set; }

        public string Motivo { get; set; } = "";

        public string Guia { get; set; } = "";

        public string Cliente { get; set; } = "";

        public decimal Cantidad { get; set; }

        public string? Detalles { get; set; }
    }
}
