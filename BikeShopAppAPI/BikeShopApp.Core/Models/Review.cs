using BikeShopApp.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Core.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

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

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
