using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public decimal TotalCost { get; set; }

        public int TotalQuantity { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
