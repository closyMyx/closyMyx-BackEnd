using closymyx.DAL.Data;
using closymyx.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace closymyx.DAL.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
           return await _db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSubCategories)
                .ThenInclude(psc => psc.SubCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}