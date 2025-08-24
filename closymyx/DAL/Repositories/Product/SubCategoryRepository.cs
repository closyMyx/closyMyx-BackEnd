using closymyx.DAL.Data;
using closymyx.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace closymyx.DAL.Repositories
{
    public class SubCategoryRepository
    {
        private readonly AppDbContext _db;

        public SubCategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<SubCategory>> GetByIdsAsync(List<Guid> ids)
        {
            return await _db.SubCategories
                .Where(sc => ids.Contains(sc.Id))
                .ToListAsync();
        }

        public async Task<List<SubCategory>> GetByProductIdAsync(Guid productId)
        {
            return await _db.ProductSubCategories
                .Where(psc => psc.ProductId == productId)
                .Select(psc => psc.SubCategory)
                .ToListAsync();
        }
    }
}
