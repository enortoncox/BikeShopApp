using BikeShopApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Dto
{
    public class ProductDto
    {
        public int? ProductId { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1000000), Required]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string ImgPath { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Range(0, 10)]
        public int AvgRating { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
