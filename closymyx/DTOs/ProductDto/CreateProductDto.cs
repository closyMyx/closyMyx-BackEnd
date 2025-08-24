namespace closymyx.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Composition { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public List<Guid> SubCategoryIds { get; set; } = new();
    }
}
