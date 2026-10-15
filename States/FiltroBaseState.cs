namespace HsqvLogistica.States
{
    public class FiltroBaseState
    {
        public string? Cliente { get; set; }

        public DateTime? FechaDesde { get; set; }
            = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        public DateTime? FechaHasta { get; set; }
            = DateTime.Today;

        public int Pagina { get; set; } = 0;
    }
}
