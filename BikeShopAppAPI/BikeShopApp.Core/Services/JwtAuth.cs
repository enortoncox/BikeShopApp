using AutoMapper;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.Enums;
using BikeShopApp.Core.Identity;
using BikeShopApp.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BikeShopApp.Core.Services
{
    public class JwtAuth : IJwtAuth
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtAuth(IConfiguration configuration, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<AuthenticationResponseDto> GenerateJwtTokenAsync(ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidOperationException("User email is required to generate a JWT.");
            }

            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expiration_Minutes"]));

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"], claims, signingCredentials: signingCredentials, expires: expiration);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var isAdmin = await _userManager.IsInRoleAsync(user, RoleTypeOptions.Admin.ToString());

            var userDto = _mapper.Map<UserDto>(user);

            return new AuthenticationResponseDto
            {
                Token = jwtToken,
                User = userDto,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["RefreshToken:Expiration_Minutes"])),
                IsAdmin = isAdmin
            };
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            try
            {
                var jwtKey = _configuration["Jwt:Key"];

                if (string.IsNullOrWhiteSpace(jwtKey))
                {
                    throw new InvalidOperationException("JWT key is not configured.");
                }

                var validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],

                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),

                    ValidateLifetime = false,

                    ClockSkew = TimeSpan.Zero
                };

                var principal = new JwtSecurityTokenHandler().ValidateToken(
                        token,
                        validationParameters,
                        out SecurityToken securityToken
                );

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private string GenerateRefreshToken() 
        {
            byte[] bytes = new byte[64];

            var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

    }
}
