namespace HsqvLogistica.Models.DTOs.Disponibilidad;

public class DisponibilidadCalendarioDto
{
    public DateOnly Fecha { get; set; }

    public decimal StockFijo { get; set; }

    public decimal CantidadReservada { get; set; }

    public decimal CantidadDisponible { get; set; }

    public decimal PorcentajeReservado { get; set; }

    public bool EstaCompleto => CantidadDisponible <= 0;
}