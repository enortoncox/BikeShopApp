using BikeShopApp.Models;

namespace BikeShopApp.Dto
{
    public class OrderDto
    {
        public int? OrderId { get; set; }

        public DateTime OrderedDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int NumOfItems { get; set; }

        public int UserId { get; set; }
    }
}
