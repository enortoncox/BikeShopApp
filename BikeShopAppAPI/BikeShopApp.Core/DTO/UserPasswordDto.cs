using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class UserPasswordDto
    {
        public int? UserId { get; set; } = null;

        [StringLength(255), Required]
        public string OldPassword { get; set; } = string.Empty;

        [StringLength(255), Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
