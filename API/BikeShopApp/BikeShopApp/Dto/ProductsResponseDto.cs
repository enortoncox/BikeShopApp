namespace BikeShopApp.Dto
{
    public class ProductsResponseDto
    {
        public ICollection<ProductDto> Products { get; set; }
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
    }
}
