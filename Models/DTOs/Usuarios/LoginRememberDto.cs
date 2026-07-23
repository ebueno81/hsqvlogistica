namespace HsqvLogistica.Models.DTOs.Usuarios
{
    public class LoginRememberDto
    {
        public string Usuario { get; set; } = "";

        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }

        public DateTime Expiration { get; set; }
    }
}
