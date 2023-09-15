
namespace BikeShopApp.Core.DTO
{
    public class AuthenticationResponseDto
    {
        public UserDto User { get; set; }

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpirationDateTime { get; set; }

        public bool IsAdmin { get; set; }
    }
}
