using Microsoft.EntityFrameworkCore;
using POS_API.Services;
using URLShortenerAPI.DTOs;
using URLShortenerAPI.Entity;

namespace URLShortenerAPI.Service
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHashService _hash;
        private readonly JwtTokenGenerateService _jwt;
        private readonly IConfiguration _config;

        public AuthService(
            AppDbContext context,
            PasswordHashService hash,
            JwtTokenGenerateService jwt,
            IConfiguration configuration)
        {
            _context = context;
            _hash = hash;
            _jwt = jwt;
            _config = configuration;
        }

        // -------------------------
        // REGISTER (PASSWORD)
        // -------------------------
        public async Task<AuthResult> RegisterAsync(RegisterRequest dto)
        {
            var exists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email);

            if (exists)
                return new AuthResult
                {
                    Status = APIStatus.Error,
                    Message = "Email already exists",
                };

            var user = new UserAccount
            {
                Email = dto.Email,
                PasswordHash = _hash.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Status = APIStatus.Success,
                Message = "User registered",
            };
        }

        // -------------------------
        // LOGIN (PASSWORD)
        // -------------------------
        public async Task<AuthResult> LoginAsync(LoginRequest dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || user.PasswordHash == null)
            {
                return new AuthResult
                {
                    Status = APIStatus.Error,
                    Message = "Invalid credentials",
                };
            }
                

            var valid = _hash.VerifyPassword(dto.Password, user.PasswordHash);

            if (!valid)
            {
                return new AuthResult
                {
                    Status = APIStatus.Error,
                    Message = "Invalid credentials",
                };
            }
                

            return GenerateToken(user);
        }

        // -------------------------
        // GOOGLE LOGIN
        // -------------------------
        public async Task<AuthResult> GoogleLoginAsync(GoogleLoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.OAuthId == dto.OAuthId);

            if (user == null)
            {
                user = new UserAccount
                {
                    Email = dto.Email,
                    OAuthId = dto.OAuthId
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return GenerateToken(user);
        }

        private AuthResult GenerateToken(UserAccount user)
        {
            var token = _jwt.GenerateToken(user.Id);

            return new AuthResult
            {
                UserId = user.Id,
                Status = APIStatus.Success,
                Message = "Authentication successful",
                Response = new APIAccessResponse
                {
                    atk = token,
                    exp = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
                }
            };
        }

    }
}