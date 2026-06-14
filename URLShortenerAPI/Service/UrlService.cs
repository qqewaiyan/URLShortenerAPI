using Microsoft.EntityFrameworkCore;
using URLShortenerAPI.Entity;

namespace URLShortenerAPI.Service
{
    public class UrlService
    {
        private readonly AppDbContext _db;
        public UrlService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<string> CreateShortUrlAsync(string originalUrl, int userId)
        {
            var code = GenerateCode();

            var url = new Url
            {
                OriginalUrl = originalUrl,
                ShortCode = code,
                UserId = userId
            };

            await _db.Urls.AddAsync(url);
            await _db.SaveChangesAsync();

            return code;
        }

        public async Task<string?> GetOriginalUrlAsync(string code)
        {
            var url = await _db.Urls.FirstOrDefaultAsync(x => x.ShortCode == code);
            return url?.OriginalUrl;
        }

        private string GenerateCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Range(0, 7)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
    }
}
