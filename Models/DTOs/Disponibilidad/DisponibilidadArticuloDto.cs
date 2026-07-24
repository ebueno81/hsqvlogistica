namespace HsqvLogistica.Models.DTOs.Disponibilidad
{
    public class DisponibilidadArticuloDto
    {
        public DisponibilidadResumenDto Resumen { get; set; } = new();

        public List<DisponibilidadCalendarioDto> Calendario { get; set; } = new();
    }
}
