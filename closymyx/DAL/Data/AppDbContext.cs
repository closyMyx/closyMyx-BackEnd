using Microsoft.EntityFrameworkCore;
using closymyx.DAL.Entities;

namespace closymyx.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<SubCategory> SubCategories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; } = null!;
        public DbSet<Favorite> Favorites { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Явные имена таблиц (избегаем зарезервированных слов)
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<SubCategory>().ToTable("SubCategories");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<ProductSubCategory>().ToTable("ProductSubCategories");
            modelBuilder.Entity<Favorite>().ToTable("Favorites");

            // Настройка ключей для связующей таблицы
            modelBuilder.Entity<ProductSubCategory>()
                .HasKey(psc => new { psc.ProductId, psc.SubCategoryId });

            // Связи
            modelBuilder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductSubCategories)
                .WithOne(psc => psc.Product)
                .HasForeignKey(psc => psc.ProductId);

            modelBuilder.Entity<SubCategory>()
                .HasMany(sc => sc.ProductSubCategories)
                .WithOne(psc => psc.SubCategory)
                .HasForeignKey(psc => psc.SubCategoryId);

            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.ProductId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites);
        }
    }
}
