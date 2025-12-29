using System.IdentityModel.Tokens.Jwt;

namespace HsqvLogistica.Integrations.Auth
{
    public class AuthTokenStore
    {
        public string? Token { get; private set; }

        public void SetToken(string token)
        {
            Token = token;
        }

        public bool HasValidToken()
        {
            if (string.IsNullOrWhiteSpace(Token))
                return false;

            try
            {
                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanReadToken(Token))
                    return false;

                var jwt = handler.ReadJwtToken(Token);

                // ⏰ exp viene en UTC
                return jwt.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }
}
