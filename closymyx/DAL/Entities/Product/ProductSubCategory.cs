namespace closymyx.DAL.Entities
{
    public class ProductSubCategory
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; } = null!;
    }
}
