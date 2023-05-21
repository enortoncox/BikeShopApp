using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Dto
{
    public class UserLoginDto
    {
        [StringLength(255), Required]
        public string Email { get; set; } = string.Empty;

        [StringLength(255), Required]
        public string Password { get; set; } = string.Empty;
    }
}
