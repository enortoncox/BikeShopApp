using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class UserDto
    {
        public int? UserId { get; set; } = null;

        [StringLength(255), MinLength(2), Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(255), Required]
        public string Address { get; set; } = string.Empty;

        [EmailAddress, StringLength(255), Required]
        public string Email { get; set; } = string.Empty;

        public int? CartId { get; set; } = null;

        [StringLength(500)]
        public string ImgPath { get; set; } = string.Empty;
    }
}
