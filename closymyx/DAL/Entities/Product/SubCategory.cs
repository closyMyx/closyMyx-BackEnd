namespace closymyx.DAL.Entities
{
    public class SubCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public List<ProductSubCategory> ProductSubCategories { get; set; } = new();
    }
}
