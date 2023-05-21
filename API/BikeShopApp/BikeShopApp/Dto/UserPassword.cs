using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Dto
{
    public class UserPassword
    {
        public int UserId { get; set; }

        [StringLength(255), Required]
        public string Password { get; set; } = string.Empty;
    }
}
