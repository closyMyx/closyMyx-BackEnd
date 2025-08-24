namespace closymyx.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Composition { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public List<string> SubCategories { get; set; } = new();
    }

}