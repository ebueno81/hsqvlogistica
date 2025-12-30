namespace HsqvLogistica.Models.DTOs.Clientes;

public class ClientePedidoDto
{
    public int IdCliente { get; set; }

    public int? IdVendedor { get; set; }

    /// <summary>
    /// Razón social / Nombre completo del cliente
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Documento (DNI / RUC)
    /// </summary>
    public string? Documento { get; set; }

    /// <summary>
    /// Dirección completa que se usará en el pedido
    /// </summary>
    public string Direccion { get; set; } = string.Empty;
}
