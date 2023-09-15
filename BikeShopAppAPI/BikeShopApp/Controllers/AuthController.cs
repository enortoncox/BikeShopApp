using AutoMapper;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using BikeShopApp.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using BikeShopApp.Core.Identity;
using BikeShopApp.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BikeShopApp.Core.Attributes;

namespace BikeShopApp.WebAPI.Controllers
{
    public class AuthController : CustomControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtAuth _jwtAuth;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// Auth Controller Constructor.
        /// </summary>
        public AuthController(IUserRepository userRepository, IMapper mapper, IJwtAuth jwtAuth, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtAuth = jwtAuth;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Allow the user to log in and return a Jwt Token.
        /// </summary>
        /// <param name="userLoginDto"></param>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto) 
        {
            if (userLoginDto == null)
            {
                return Problem(detail:"User Login details are null.", title: "Bad Request", statusCode: 400);
            }

            ApplicationUser? user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email);

            if (user == null) 
            {
                return Problem(detail: "Incorrect Credentials.", title: "Not Found", statusCode: 404);
            }

            var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded) 
            {
                return Problem(detail: "Incorrect Credentials.", title: "Not Found", statusCode: 404);
            }

            AuthenticationResponseDto authenticationResponse = await _jwtAuth.GenerateJwtTokenAsync(user);

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        /// <summary>
        /// Register a new user and return a Jwt Token.
        /// </summary>
        /// <param name="userRegisterDto"></param>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null)
            {
                return Problem(detail: "User Register details are null.", title: "Bad Request", statusCode: 400);
            }

            ApplicationUser? user = _mapper.Map<ApplicationUser>(userRegisterDto);

            user.UserName = userRegisterDto.Email;

            user = await _userRepository.CreateUserAsync(user, userRegisterDto.Password);

            if (user == null) 
            {
                return Problem(detail: "Something went wrong while registering a new user.", statusCode: 500, title: "Internal Server Error");
            }

            if (!await _roleManager.RoleExistsAsync(RoleTypeOptions.User.ToString())) 
            {
                ApplicationRole userRole = new ApplicationRole() { Name = RoleTypeOptions.User.ToString() };
                await _roleManager.CreateAsync(userRole);
            }

            await _userManager.AddToRoleAsync(user, RoleTypeOptions.User.ToString());

            await _signInManager.SignInAsync(user, isPersistent: false);

            AuthenticationResponseDto authenticationResponse = await _jwtAuth.GenerateJwtTokenAsync(user);

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }


        /// <summary>
        /// Check if the user with the passed Id has the "Admin" role.
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("isadmin/{userId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> IsUserAdmin(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return Problem(detail: $"A user with the Id of {userId} was not found.", title: "Not Found", statusCode: 404);
            }

            var result = await _userRepository.IsUserAdminAsync(userId);

            return Ok(result);
        }

        /// <summary>
        /// Remove or Add the "Admin" role to a user.
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("isadmin/{userId}/change")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> ChangeAdminStatus(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return Problem(detail: $"A user with the Id of {userId} was not found.", title: "Not Found", statusCode: 404);
            }

            if (!await _userRepository.ChangeAdminStatusAsync(userId))
            {
                return Problem(detail: "Something went wrong while changing the admin status.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Generate a new Jwt and refresh token and return it.
        /// </summary>
        /// <param name="tokensDto"></param>
        [HttpPost("generate-new-jwt-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateNewJwtToken(TokensDto tokensDto)
        {
            if (tokensDto.JwtToken == "" || tokensDto.RefreshToken == "") 
            {
                return Problem(detail: "Invalid tokens.", title: "Bad Request", statusCode: 400);
            }

            ClaimsPrincipal? principal;

            principal = _jwtAuth.GetPrincipalFromToken(tokensDto.JwtToken);

            if (principal == null) 
            {
                return Problem(detail: $"Invalid tokens.", title: "Bad Request", statusCode: 400);
            }

            ApplicationUser? user = await _userRepository.GetUserByEmailAsync(principal.FindFirstValue(ClaimTypes.Email));

            if (user == null || user.RefreshToken != tokensDto.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow) 
            {
                return Problem(detail: $"Invalid tokens.", title: "Bad Request", statusCode: 400);
            }

            var authenticationResponse = await _jwtAuth.GenerateJwtTokenAsync(user);

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        /// <summary>
        /// Verify if the passed tokens are valid.
        /// </summary>
        /// <param name="tokensDto"></param>
        [HttpPost("verify-jwt-token")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyJwtToken(TokensDto tokensDto)
        {           
            if (tokensDto.JwtToken == "" || tokensDto.RefreshToken == "")
            {
                return Problem(detail: $"Invalid tokens.", title: "Bad Request", statusCode: 400);
            }

            VerifyTokenResponseDto tokenResponse = new VerifyTokenResponseDto();

            ClaimsPrincipal? principal;

            principal = _jwtAuth.GetPrincipalFromToken(tokensDto.JwtToken);

            if (principal == null) 
            {
                return Ok(tokenResponse);
            }

            ApplicationUser? user = await _userRepository.GetUserByEmailAsync(principal.FindFirstValue(ClaimTypes.Email));

            if (user == null) 
            {
                return Ok(tokenResponse);
            }

            if (user.RefreshToken != tokensDto.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
            {
                tokenResponse.RefreshValid = false;
                return Ok(tokenResponse);
            }
            else 
            {
                tokenResponse.RefreshValid = true;
            }

            var expirySeconds = Convert.ToInt32(principal.FindFirstValue("exp"));

            DateTime expiryTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(expirySeconds);

            if (expiryTime <= DateTime.UtcNow)
            {
                tokenResponse.JwtValid = false;
            }
            else 
            {
                tokenResponse.JwtValid = true;
            }

            tokenResponse.User = _mapper.Map<UserDto>(user);
            tokenResponse.IsAdmin = await _userManager.IsInRoleAsync(user, RoleTypeOptions.Admin.ToString());
            return Ok(tokenResponse);            
        }
    }
}
