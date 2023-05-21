using AutoMapper;
using BikeShopApp.Dto;
using BikeShopApp.Interfaces;
using BikeShopApp.Models;
using BikeShopApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userAuth) 
        {
            if (userAuth == null) 
            {
                return BadRequest("User Login details are null.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var foundUser = await _userRepository.GetUserByEmailAsync(userAuth.Email);

            if (foundUser == null) 
            {
               return NotFound("Incorrect Credentials");
            }

            if (foundUser.Password != userAuth.Password) 
            {
                return NotFound("Incorrect Credentials");
            }

            var mappedUser = _mapper.Map<UserDto>(foundUser);

            return Ok(new { mappedUser });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {
            if (userRegister == null)
            {
                return BadRequest("User Register details are null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedRegisteredUser = _mapper.Map<User>(userRegister);

            //Still to implement true Authentication and Authorization. Plaintext password will be replaced by hash.
            var createdMappedUser = _mapper.Map<UserDto>(await _userRepository.CreateUserAsync(mappedRegisteredUser));

            if (createdMappedUser == null) 
            {
                ModelState.AddModelError("", "Something went wrong registering a new user");
                return StatusCode(500, ModelState);
            }

            return Ok(new { createdMappedUser });
        }


        [HttpPost("Password")]
        public async Task<IActionResult> ConfirmUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
            {
                return BadRequest("userPassword was null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _userRepository.UserExistsAsync(userPassword.UserId)) 
            {
                return NotFound($"A user with the Id of {userPassword.UserId} was not found.");
            }

            var user = await _userRepository.GetUserByIdAsync(userPassword.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "Something went wrong checking the Password");
                return StatusCode(500, ModelState);
            }

            if (user.Password == userPassword.Password)
            {
                return Ok(true);
            }
            else 
            {
                return Ok(false);
            }
        }

        [HttpGet("isadmin/{userId}")]
        public async Task<IActionResult> IsUserAdmin(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"A user with the Id of {userId} was not found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userRepository.IsUserAdminAsync(userId))
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpGet("isadmin/{userId}/change")]
        public async Task<IActionResult> ChangeAdminStatus(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"A user with the Id of {userId} was not found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _userRepository.ChangeAdminStatusAsync(userId))
            {
                ModelState.AddModelError("", "Something went wrong changing the admin status");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
