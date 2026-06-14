namespace URLShortenerAPI.Entity
{
    public class UserAccount
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string? OAuthId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Url> Urls { get; set; } = new();
    }
}
