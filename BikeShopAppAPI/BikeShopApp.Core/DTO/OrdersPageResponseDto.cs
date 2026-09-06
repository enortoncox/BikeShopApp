namespace BikeShopApp.Core.DTO
{
    public class OrdersPageResponseDto
    {
        public ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public int CurrentPage { get; set; }
        public int Pages { get; set; }


    }
}
