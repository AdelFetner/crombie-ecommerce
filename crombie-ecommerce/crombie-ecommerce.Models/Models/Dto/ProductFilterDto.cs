namespace crombie_ecommerce.Models.Models.Dto
{
    // this helps with making the filtering flexible, so it doesn't need tons of overloaded methods
    public class ProductFilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<Guid>? BrandIds { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsOnWishlist { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
