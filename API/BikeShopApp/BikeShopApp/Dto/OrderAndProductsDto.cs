namespace BikeShopApp.Dto
{
    public class OrderAndProductsDto
    {
        public OrderDto Order { get; set; }

        public ICollection<int> ProductsIds { get; set; }
    }
}
