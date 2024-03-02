namespace Pattern.Application.Services.Authentication.Dtos
{
    public class AccessTokenDto
    {
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpiration { get; set; }
    }
}
