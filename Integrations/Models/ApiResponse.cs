namespace HsqvLogistica.Integrations.Models.Clientes;

public class ApiResponse<T>
{
    public bool Status { get; set; }
    public string? Msg { get; set; }
    public T? Value { get; set; }
}
