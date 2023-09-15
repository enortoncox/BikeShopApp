using BikeShopApp.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class ReviewDto
    {
        public int? ReviewId { get; set; } = null;

        [StringLength(255)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; } = string.Empty;

        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        [Required(ErrorMessage = "Rating is required")]
        public int? Rating { get; set; } = null;

        [Required(ErrorMessage = "UserId is required")]
        public int? UserId { get; set; } = null;

        [Required(ErrorMessage = "ProductId is required")]
        public int? ProductId { get; set; } = null;
    }
}
