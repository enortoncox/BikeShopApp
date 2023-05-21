namespace BikeShopApp.Dto
{
    public class OrdersResponseDto
    {
        public ICollection<OrderDto> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int Pages { get; set; }


    }
}
