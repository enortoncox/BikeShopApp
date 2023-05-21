using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderedDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int NumOfItems { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
