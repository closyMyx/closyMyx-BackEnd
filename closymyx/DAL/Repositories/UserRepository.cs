using closymyx.DAL.Data;
using closymyx.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace closymyx.DAL.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            _db.User.Add(user);                 
            await _db.SaveChangesAsync();       
        }
    }
}
