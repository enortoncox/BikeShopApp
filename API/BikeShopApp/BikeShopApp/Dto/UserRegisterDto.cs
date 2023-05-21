using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Dto
{
    public class UserRegisterDto
    {
        [StringLength(255), Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(255), Required]
        public string Address { get; set; } = string.Empty;

        [EmailAddress, StringLength(255), Required]
        public string Email { get; set; } = string.Empty;

        [StringLength(500)]
        public string ImgPath { get; set; } = string.Empty;

        [StringLength(255), Required]
        public string Password { get; set; } = string.Empty;
    }
}
