using BikeShopApp.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Core.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderedDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int NumOfItems { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
