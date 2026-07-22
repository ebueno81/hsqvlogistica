using HsqvLogistica.Common;

namespace HsqvLogistica.Helpers
{
    public static class PermissionHelper
    {
        public static PermisosUsuario ObtenerPermisos(int idTipo)
        {
            var permisos = new PermisosUsuario();

            switch (idTipo)
            {
                case 1: // Logística
                case 2: // Vendedor

                    permisos.PuedeVerMantenimiento = true;
                    permisos.PuedeVerOperaciones = true;
                    permisos.PuedeVerReportes = true;
                    permisos.PuedeEditarGarantia = true;
                    permisos.PuedeCrearPedidos = true;

                    break;

                case 3: // Supervisor

                    permisos.PuedeVerMantenimiento = true;
                    permisos.PuedeVerOperaciones = true;
                    permisos.PuedeVerReportes = true;
                    permisos.PuedeCrearPedidos = true;

                    permisos.PuedeAprobarPedidos = true;
                    permisos.PuedeAnularOperaciones = true;
                    permisos.PuedeCrearArticulos = true;
                    permisos.PuedeCrearLineas = true;
                    permisos.PuedeEditarGarantia = true;

                    break;

                case 4: // Administrador

                    permisos.PuedeVerMantenimiento = true;
                    permisos.PuedeVerOperaciones = true;
                    permisos.PuedeVerReportes = true;
                    permisos.PuedeCrearPedidos = true;

                    permisos.PuedeAdministrarUsuarios = true;
                    permisos.PuedeAprobarPedidos = true;
                    permisos.PuedeAnularOperaciones = true;
                    permisos.PuedeCrearArticulos = true;
                    permisos.PuedeCrearMotivos = true;
                    permisos.PuedeCrearLineas = true;
                    permisos.PuedeEditarGarantia = true;

                    break;
            }

            return permisos;
        }
    }
}
