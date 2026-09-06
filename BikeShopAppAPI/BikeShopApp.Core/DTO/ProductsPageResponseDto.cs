namespace BikeShopApp.Core.DTO
{
    public class ProductsPageResponseDto
    {
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
    }
}
