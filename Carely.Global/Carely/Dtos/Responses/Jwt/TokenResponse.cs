namespace Carely.Dtos.Responses.Jwt
{
    public class TokenResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
