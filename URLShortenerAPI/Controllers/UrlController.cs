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
            if(req.userId <=0 )
            {
                return BadRequest("UserId is required");
            }
            var code = await _service.CreateShortUrlAsync(req.Url, req.userId);

            var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}";

            return Ok(new
            {
                shortUrl
            });
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectToUrl(string code)
        {
            var url = await _service.GetOriginalUrlAsync(code);

            if (url is null)
                return NotFound();

            return Redirect(url);
        }
    }
}
