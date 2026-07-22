using HsqvLogistica.Common.Configuration;
using HsqvLogistica.Models.DTOs.Movimientos;
using HsqvLogistica.Models.DTOs.Pedidos;
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

        #region Métodos privados

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

        #endregion
    }
}