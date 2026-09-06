using BikeShopApp.Core.DTO;
using BikeShopApp.Core.Identity;
using System.Security.Claims;

namespace BikeShopApp.Core.ServiceInterfaces
{
    public interface IJwtAuth
    {
        Task<AuthenticationResponseDto> GenerateJwtTokenAsync(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}
