namespace HsqvLogistica.Common
{
    public class PermisosUsuario
    {
        public bool PuedeVerMantenimiento { get; set; }

        public bool PuedeVerOperaciones { get; set; }

        public bool PuedeVerReportes { get; set; }

        public bool PuedeAdministrarUsuarios { get; set; }

        public bool PuedeAprobarPedidos { get; set; }

        public bool PuedeAnularOperaciones { get; set; }

        public bool PuedeCrearArticulos { get; set; }

        public bool PuedeCrearMotivos { get; set; }

        public bool PuedeCrearLineas { get; set; }

        public bool PuedeCrearPedidos { get; set; }

        public bool PuedeCrearMovimientos { get; set; }

        public bool PuedeCrearGarantia { get; set; }

        public bool PuedeEditarGarantia { get; set; }

    }
}
