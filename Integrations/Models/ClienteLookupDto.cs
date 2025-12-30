namespace HsqvLogistica.Integrations.Models.Clientes;

public class ClienteLookupDto
{
    public int IdCliente { get; set; }
    public string? NombresApellidos { get; set; }
    public string? NroDoc { get; set; }
    public string? Celular { get; set; }
    public string? Distrito { get; set; }
    public int? IdVendedor { get; set; }
}
