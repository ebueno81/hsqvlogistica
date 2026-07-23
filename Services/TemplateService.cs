using HsqvLogistica.Common.Configuration;
using HsqvLogistica.Models.DTOs.Garantia;
using HsqvLogistica.Models.DTOs.Garantias;
using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.DTOs.Pedidos;
using HsqvLogistica.Models.DTOs.Usuarios;
using HsqvLogistica.Services.Interfaces;
using System.Text;

namespace HsqvLogistica.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IWebHostEnvironment _environment;

        private const string TemplateFolder = "Common/Email/EmailTemplates";

        public TemplateService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        #region Pedidos

        public async Task<string> PedidoCreadoAsync(
            PedidoDto pedido,
            NotificationSettings settings)
        {
            var html = await LeerPlantillaAsync("PedidoCreado.html");

            return ReemplazarDatosComunes(
                html,
                pedido,
                settings);
        }

        public async Task<string> PedidoAprobadoAsync(
            PedidoDto pedido,
            NotificationSettings settings)
        {
            var html = await LeerPlantillaAsync("PedidoAprobado.html");

            html = ReemplazarDatosComunes(
                html,
                pedido,
                settings);

            html = html.Replace(
                "{{UsuarioAprueba}}",
                pedido.UsuarioModifica ?? string.Empty);

            html = html.Replace(
                "{{FechaAprobacion}}",
                pedido.FechaModifica?.ToString("dd/MM/yyyy HH:mm") ?? string.Empty);

            return html;
        }

        #endregion

        #region Movimientos

        public Task<string> SalidaAlmacenAsync(
            MovimientoDto movimiento,
            NotificationSettings settings)
        {
            throw new NotImplementedException();
        }

        public Task<string> DevolucionAsync(
            MovimientoDto movimiento,
            NotificationSettings settings)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Garantías

        public async Task<string> GarantiaCreadaAsync(
            GarantiaDto garantia,
            NotificationSettings settings)
        {
            var html = await LeerPlantillaAsync("GarantiaCreada.html");

            return ReemplazarDatosGarantia(
                html,
                garantia,
                settings);
        }

        public async Task<string> GarantiaCerradaAsync(GarantiaDto garantia, NotificationSettings settings)
        {
            var html = await LeerPlantillaAsync("GarantiaCerrada.html");

            html = ReemplazarDatosGarantia(
                html,
                garantia,
                settings);

            html = html.Replace(
                "{{UsuarioCierre}}",
                garantia.UsuaModifica ?? string.Empty);

            html = html.Replace(
                "{{FechaCierre}}",
                garantia.FechaModifica?.ToString("dd/MM/yyyy HH:mm") ?? string.Empty);

            return html;
        }

        #endregion

        #region Métodos privados

        private string ReemplazarDatosGarantia(
            string html,
            GarantiaDto garantia,
            NotificationSettings settings)
        {
            html = html.Replace(
                "{{Numero}}",
                garantia.Id.ToString());

            html = html.Replace(
                "{{Cliente}}",
                garantia.Cliente ?? string.Empty);

            html = html.Replace(
                "{{Fecha}}",
                garantia.FechaDespacho?.ToString("dd/MM/yyyy") ?? string.Empty);

            html = html.Replace(
                "{{Observacion}}",
                garantia.Detalles ?? string.Empty);

            var returnUrl = Uri.EscapeDataString(
                $"/garantias/ver/{garantia.Id}");

            html = html.Replace(
                "{{UrlGarantia}}",
                $"{settings.UrlSistema}/login?returnUrl={returnUrl}");

            html = html.Replace(
                "{{Detalle}}",
                GenerarTablaGarantia(garantia.DetallesGarantia));

            return html;
        }

        private async Task<string> LeerPlantillaAsync(string archivo)
        {
            if (string.IsNullOrWhiteSpace(archivo))
                throw new ArgumentException(
                    "Debe indicar el nombre de la plantilla.",
                    nameof(archivo));

            var ruta = Path.Combine(
                _environment.ContentRootPath,
                TemplateFolder,
                archivo);

            if (!File.Exists(ruta))
                throw new FileNotFoundException(
                    $"No existe la plantilla: {ruta}");

            return await File.ReadAllTextAsync(ruta);
        }

        private string ReemplazarDatosComunes(
            string html,
            PedidoDto pedido,
            NotificationSettings settings)
        {
            html = html.Replace(
                "{{Numero}}",
                pedido.Id.ToString());

            html = html.Replace(
                "{{Cliente}}",
                pedido.Cliente ?? string.Empty);

            html = html.Replace(
                "{{Fecha}}",
                pedido.FechaEntrega?.ToString("dd/MM/yyyy") ?? string.Empty);

            html = html.Replace(
                "{{Direccion}}",
                pedido.Direccion ?? string.Empty);

            var returnUrl = Uri.EscapeDataString(
                $"/pedidos/ver/{pedido.Id}");

            html = html.Replace(
                "{{UrlPedido}}",
                $"{settings.UrlSistema}/login?returnUrl={returnUrl}");

            html = html.Replace(
                "{{Detalle}}",
                GenerarTabla(pedido));

            return html;
        }

        private string GenerarTabla(PedidoDto pedido)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
                <table>

                    <thead>

                        <tr>

                            <th style='text-align:left'>Código</th>
                            <th style='text-align:left'>Artículo</th>
                            <th style='text-align:center'>Cantidad</th>
                            <th style='text-align:left'>Unidad</th>

                        </tr>

                    </thead>

                    <tbody>");

            foreach (var item in pedido.Detalle ?? Enumerable.Empty<PedidoDetalleDto>())
            {
                sb.AppendLine($@"
                <tr>
                    <td>{item.IdArticulo}</td>
                    <td>{item.Articulo}</td>
                    <td align='center'>{item.Cantidad}</td>
                    <td>Unid.</td>
                </tr>");
            }

            sb.AppendLine(@"
                </tbody>

            </table>");

            return sb.ToString();
        }

        private string GenerarTablaGarantia(IEnumerable<GarantiaDetalleDto> detalles)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
                <table>

                    <thead>

                        <tr>

                            <th style='text-align:left'>Código</th>
                            <th style='text-align:left'>Artículo</th>
                            <th style='text-align:center'>Cantidad</th>
                            <th style='text-align:left'>Unidad</th>
                            <th style='text-align:left'>Detalle</th>

                        </tr>

                    </thead>

                    <tbody>");

                            foreach (var item in detalles ?? Enumerable.Empty<GarantiaDetalleDto>())
                            {
                                sb.AppendLine($@"
                                <tr>
                                    <td>{item.IdArticulo}</td>
                                    <td>{item.Articulo}</td>
                                    <td align='center'>{item.Cantidad}</td>
                                    <td>{item.Unidad}</td>
                                    <td>{item.Detalles}</td>
                                </tr>");
                            }

                            sb.AppendLine(@"

                    </tbody>

                </table>");

            return sb.ToString();
        }

        public async Task<string> PasswordRecoveryAsync(UsuarioDto usuario, NotificationSettings settings)
        {
            var html = await LeerPlantillaAsync("PasswordRecovery.html");

            html = ReemplazarConfiguracion(html, settings);

            html = html.Replace(
                "{{Nombre}}",
                usuario.Nombres ?? string.Empty);

            html = html.Replace(
                "{{Usuario}}",
                usuario.Usuario ?? string.Empty);

            html = html.Replace(
                "{{Password}}",
                usuario.Password ?? string.Empty);

            return html;
        }

        private string ReemplazarConfiguracion(string html, NotificationSettings settings)
                {
                    html = html.Replace(
                        "{{Empresa}}",
                        settings.Empresa ?? string.Empty);

                    html = html.Replace(
                        "{{UrlSistema}}",
                        settings.UrlSistema ?? string.Empty);

                    return html;
        }

        public async Task<string> UsuarioCreadoAsync(UsuarioDto usuario,
            NotificationSettings settings)
        {
            var html = await LeerPlantillaAsync("UsuarioCreado.html");

            return ReemplazarDatosUsuario(
                html,
                usuario,
                settings);
        }

        public async Task<string> UsuarioActualizadoAsync(UsuarioDto usuario,
            NotificationSettings settings)
        {
            var html = await LeerPlantillaAsync("UsuarioActualizado.html");

            return ReemplazarDatosUsuario(
                html,
                usuario,
                settings);
        }

        private string ReemplazarDatosUsuario(string html, UsuarioDto usuario,
            NotificationSettings settings)
        {
            html = html.Replace(
                "{{Empresa}}",
                settings.Empresa ?? string.Empty);

            html = html.Replace(
                "{{Nombre}}",
                usuario.Nombres ?? string.Empty);

            html = html.Replace(
                "{{Usuario}}",
                usuario.Usuario ?? string.Empty);

            html = html.Replace(
                "{{Clave}}",
                usuario.Password ?? string.Empty);

            html = html.Replace(
                "{{Correo}}",
                usuario.Correo ?? string.Empty);

            html = html.Replace(
                "{{TipoUsuario}}",
                usuario.TipoUsuario ?? string.Empty);

            html = html.Replace(
                "{{UrlSistema}}",
                settings.UrlSistema ?? string.Empty);

            return html;
        }
        #endregion
    }
}