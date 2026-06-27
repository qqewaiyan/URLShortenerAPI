using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShortenerAPI.DTOs;
using URLShortenerAPI.Service;

namespace URLShortenerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlService _service;

        public UrlController(UrlService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("shorten")]
        public async Task<IActionResult> Shorten([FromBody] CreateUrlRequest req)
        {
            if (req.userId <= 0)
            {
                return BadRequest("UserId is required");
            }

            var url = await _service.CreateShortUrlAsync(req.Url, req.userId) ?? new Entity.Url();

            var shortUrl = $"{Request.Scheme}://{Request.Host}/{url.ShortCode}";

            return Ok(new
            {
                id = url.Id,
                originalUrl = url.OriginalUrl,
                shortCode = url.ShortCode,
                shortUrl,
                clicks = url.ClickCount,
                createdAt = url.CreatedAt,
                userId = url.UserId
            });
        }

        [HttpGet("/user/{uid}")]
        public async Task<IActionResult> GetAll(int uid)
        {
            var urls = await _service.GetAllUrl(uid);

            return Ok(urls);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectToUrl(string code)
        {
            var url = await _service.GetOriginalUrlAsync(code);

            if (url is null)
                return NotFound();

            return Redirect(url);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
