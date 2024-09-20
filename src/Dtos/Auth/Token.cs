namespace src.Dtos.Auth
{
    public class Token
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
