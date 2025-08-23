using Microsoft.EntityFrameworkCore;
using closymyx.DAL.Entities;

namespace closymyx.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User => Set<User>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    }
}
