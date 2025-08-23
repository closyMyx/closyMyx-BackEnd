using closymyx.DTOs;
using closymyx.DAL.Entities;
using closymyx.DAL.Repositories;
using BCrypt.Net;

namespace closymyx.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

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
    }
}