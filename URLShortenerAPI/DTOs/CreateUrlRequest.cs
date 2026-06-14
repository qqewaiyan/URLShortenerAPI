namespace URLShortenerAPI.DTOs
{
    public class CreateUrlRequest
    {
        public string Url { get; set; } = string.Empty;
        public int userId { get; set; }
    }
}
