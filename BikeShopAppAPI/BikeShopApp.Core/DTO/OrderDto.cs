using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class OrderDto
    {
        public int? OrderId { get; set; } = null;

        [Required(ErrorMessage = "OrderedDate is required")]
        public DateTime? OrderedDate { get; set; } = null;

        [Required(ErrorMessage = "TotalPrice is required")]
        public decimal? TotalPrice { get; set; } = null;

        [Required(ErrorMessage = "Number of Items is required")]
        public int? NumOfItems { get; set; } = null;

        [Required(ErrorMessage = "UserId is required")]
        public int? UserId { get; set; } = null;
    }
}
