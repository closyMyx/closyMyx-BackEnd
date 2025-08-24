using closymyx.DAL.Data;
using closymyx.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace closymyx.DAL.Repositories
{
    public class CategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) => _db = db;

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
