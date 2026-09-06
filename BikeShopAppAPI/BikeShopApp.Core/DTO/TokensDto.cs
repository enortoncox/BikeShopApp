using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class TokensDto
    {
        public string? JwtToken { get; set; }

        [Required]
        public string? RefreshToken { get; set; }
    }
}
