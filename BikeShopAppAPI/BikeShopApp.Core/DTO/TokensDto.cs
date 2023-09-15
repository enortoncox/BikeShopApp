using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class TokensDto
    {
        [Required]
        public string JwtToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
