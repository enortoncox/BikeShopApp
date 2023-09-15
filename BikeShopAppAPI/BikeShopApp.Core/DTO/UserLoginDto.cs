using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class UserLoginDto
    {
        [StringLength(255, ErrorMessage = "Email is too long")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Password is too long")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
