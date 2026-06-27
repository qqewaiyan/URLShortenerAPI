using Microsoft.EntityFrameworkCore;
using URLShortenerAPI.DTOs;
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
        public async Task<Url?> CreateShortUrlAsync(string originalUrl, int userId)
        {
            var code = GenerateCode();

            var url = new Url
            {
                OriginalUrl = originalUrl,
                ShortCode = code,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            await _db.Urls.AddAsync(url);
            await _db.SaveChangesAsync();

            return url;
        }

        public async Task<List<Url>?> GetAllUrl(int uid)
        {
            var urls = await _db.Urls.Where(x => x.UserId == uid).ToListAsync();
            return urls;
        }

        public async Task<string?> GetOriginalUrlAsync(string code)
        {
            var url = await _db.Urls.FirstOrDefaultAsync(x => x.ShortCode == code);
            if(url is not null)
            {
                url.ClickCount += 1;
                await _db.SaveChangesAsync();
            }
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
        public async Task<bool> DeleteAsync(int id)
        {
            var url = await _db.Urls.FindAsync(id);

            if (url == null)
                return false;

            _db.Urls
                .Remove(url);

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
