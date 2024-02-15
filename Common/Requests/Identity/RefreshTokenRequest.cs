namespace Common.Requests.Identity
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
    }
}
