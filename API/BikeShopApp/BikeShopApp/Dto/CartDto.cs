using BikeShopApp.Models;

namespace BikeShopApp.Dto
{
    public class CartDto
    {
        public int? CartId { get; set; }

        public decimal TotalCost { get; set; }

        public int TotalQuantity { get; set; }

        public int UserId { get; set; }
    }
}
