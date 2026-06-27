using URLShortenerAPI.Entity;

namespace URLShortenerAPI.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = null!;
        public string? OAuthId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Url> Urls { get; set; } = new();
    }
}
