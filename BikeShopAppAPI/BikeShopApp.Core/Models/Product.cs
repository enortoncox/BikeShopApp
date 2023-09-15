using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }

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

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
