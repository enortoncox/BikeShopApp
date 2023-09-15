using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class ProductDto
    {
        public int? ProductId { get; set; } = null;

        [StringLength(255, ErrorMessage = "Name is too long")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "Price must be greater than 0 and less than 1000000")]
        [Required(ErrorMessage = "Price is required")]
        public decimal? Price { get; set; } = null;

        [StringLength(500)]
        public string ImgPath { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        public int? Quantity { get; set; } = null;

        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        public int AvgRating { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int? CategoryId { get; set; } = null;
    }
}
