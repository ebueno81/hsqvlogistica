namespace HsqvLogistica.Integrations.Models.Auth
{
    public class UsuarioLoginResponseDto
    {
        public bool Status { get; set; }
        public string Msg { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
