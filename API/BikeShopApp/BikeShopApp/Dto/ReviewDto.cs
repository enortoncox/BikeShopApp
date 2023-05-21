using BikeShopApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Dto
{
    public class ReviewDto
    {
        public int? ReviewId { get; set; }

        [StringLength(255), Required]
        public string Title { get; set; } = string.Empty;

        [StringLength(500), Required]
        public string Text { get; set; } = string.Empty;

        [Range(0, 10), Required]
        public int Rating { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
