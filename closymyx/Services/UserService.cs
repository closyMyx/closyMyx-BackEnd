using closymyx.DAL.Entities;
using closymyx.DAL.Repositories;
using closymyx.DTOs;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace closymyx.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;
        private readonly string _jwtSecret = "jwtTokenForAppSuperSecretKey123!"; // минимум 128 бит для HMAC

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto dto)
        {
            var existing = await _userRepo.GetByEmailAsync(dto.Email);
            if (existing != null) return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            await _userRepo.AddAsync(user);
            return true;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new AuthResponseDto
            {
                Email = user.Email,
                Token = jwt
            };
        }
    }
}
