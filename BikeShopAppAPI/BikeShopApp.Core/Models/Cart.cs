using BikeShopApp.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Core.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public decimal TotalCost { get; set; }

        public int TotalQuantity { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
