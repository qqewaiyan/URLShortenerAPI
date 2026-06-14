namespace URLShortenerAPI.DTOs
{
    public class AuthResult
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public APIAccessResponse? Response { get; set; }
        public int UserId { get; set; }
    }
}
