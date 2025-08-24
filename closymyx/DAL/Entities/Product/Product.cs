namespace closymyx.DAL.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Composition { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public List<ProductSubCategory> ProductSubCategories { get; set; } = new();

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
