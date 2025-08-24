namespace closymyx.DAL.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;

        public List<SubCategory> SubCategories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }
}
