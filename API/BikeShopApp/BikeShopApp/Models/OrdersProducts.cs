namespace BikeShopApp.Models
{
    public class OrdersProducts
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
