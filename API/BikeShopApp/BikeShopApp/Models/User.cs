using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [StringLength(255), MinLength(2), Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(255), Required]
        public string Address { get; set; } = string.Empty;

        [EmailAddress, StringLength(255), Required]
        public string Email { get; set; } = string.Empty;

        public int CartId { get; set; }

        [StringLength(500)]
        public string ImgPath { get; set; } = string.Empty;

        //Still to implement true Authentication and Authorization. Plaintext password will be replaced by hash.
        [MaxLength(255), Required]
        public string Password { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        public Cart Cart { get; set; }

        public List<Order> Orders { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
