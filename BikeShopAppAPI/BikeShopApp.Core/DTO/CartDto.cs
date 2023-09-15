using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class CartDto
    {
        public int? CartId { get; set; } = null;

        [Required(ErrorMessage = "Total Cost is required")]
        public decimal TotalCost { get; set; }

        [Required(ErrorMessage = "Total Quantity is required")]
        public int TotalQuantity { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int? UserId { get; set; } = null;
    }
}
