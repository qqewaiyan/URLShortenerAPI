namespace URLShortenerAPI.Entity
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = null!;
        public string ShortCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int ClickCount { get; set; }

        // Foreign key
        public int UserId { get; set; }

        // Navigation
        public UserAccount User { get; set; } = null!;
    }
}
