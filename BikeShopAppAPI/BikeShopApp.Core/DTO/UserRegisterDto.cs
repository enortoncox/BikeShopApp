using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class UserRegisterDto
    {
        [StringLength(255, ErrorMessage = "Name is too long")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Address is too long")]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email must be in valid email format")]
        [StringLength(255, ErrorMessage = "Email is too long")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Img Path is too long")]
        public string ImgPath { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Password is too long")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Password is too long")]
        [Required(ErrorMessage = "Password is required")]
        [Compare("Password", ErrorMessage = "Confirm Password must match password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
