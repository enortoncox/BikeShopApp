
namespace BikeShopApp.Core.DTO
{
    public class VerifyTokenResponseDto
    {
        public UserDto? User { get; set; } = null;
        public bool IsAdmin { get; set; } = false;
        public bool RefreshValid { get; set; } = false;
        public bool JwtValid { get; set; } = false;
    }
}
