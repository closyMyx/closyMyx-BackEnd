using closymyx.DAL.Entities;
using closymyx.DAL.Repositories;
using closymyx.DTOs;

namespace closymyx.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repo;
        private readonly CategoryRepository _categoryRepo; // нужен для проверки
        private readonly SubCategoryRepository _subCategoryRepo; // нужен для проверки

        public ProductService(ProductRepository repo,
                              CategoryRepository categoryRepo,
                              SubCategoryRepository subCategoryRepo)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
            _subCategoryRepo = subCategoryRepo;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new Exception($"Категория с Id {dto.CategoryId} не найдена");

            var subCategories = await _subCategoryRepo.GetByIdsAsync(dto.SubCategoryIds);
            if (subCategories.Count != dto.SubCategoryIds.Count)
                throw new Exception("Одна или несколько подкатегорий не найдены");

            var product = new Product
            {
                Name = dto.Name,
                Composition = dto.Composition,
                CategoryId = dto.CategoryId
            };

            product.ProductSubCategories = subCategories
                .Select(sc => new ProductSubCategory
                {
                    Product = product,
                    SubCategoryId = sc.Id
                }).ToList();

            await _repo.AddAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Composition = product.Composition,
                CategoryName = category.Name,
                SubCategories = subCategories.Select(sc => sc.Name).ToList()
            };
        }

        public async Task<List<ProductDto>> GetSimilarProductsAsync(Guid productId)
        {
            var product = await _repo.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Товар не найден");

            var category = await _categoryRepo.GetByIdAsync(product.CategoryId);

            // Разбиваем состав (Composition) на слова
            var keywords = (product.Composition ?? "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(w => w.ToLower().Substring(0, Math.Min(5, w.Length))) // берём основу
                .Distinct()
                .ToList();

            var allProducts = await _repo.GetByCategoryIdAsync(product.CategoryId);

            var similar = allProducts
                .Where(p => p.Id != product.Id && !string.IsNullOrWhiteSpace(p.Composition))
                .Where(p => keywords.Any(k =>
                    p.Composition!.ToLower()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Any(word => word.StartsWith(k))
                ))
                .Take(10)
                .ToList();

            // Загружаем подкатегории
            var result = new List<ProductDto>();
            foreach (var sim in similar)
            {
              var subs = await _subCategoryRepo.GetByProductIdAsync(sim.Id);

                result.Add(new ProductDto
                {
                    Id = sim.Id,
                    Name = sim.Name,
                    Composition = sim.Composition,
                    CategoryName = category?.Name ?? "",
                    SubCategories = subs.Select(sc => sc.Name).ToList()
                });
            }

            return result;
        }
    }
}
