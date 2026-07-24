namespace HsqvLogistica.Models.DTOs.Articulos
{
    public class StockArticuloDto
    {
        public int IdArticulo { get; set; }

        public string Codigo { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public decimal Stock { get; set; }
    }
}