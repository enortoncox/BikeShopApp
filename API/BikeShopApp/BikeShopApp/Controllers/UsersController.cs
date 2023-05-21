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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers() 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedUsers = _mapper.Map<List<UserDto>>(await _userRepository.GetUsersAsync());

            if (mappedUsers == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the users.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedUsers);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId)) 
            {
                return NotFound($"No user with the Id of {userId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedUser = _mapper.Map<UserDto>(await _userRepository.GetUserByIdAsync(userId));

            if (mappedUser == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the user.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedUser);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDto updatedUser) 
        {
            if (updatedUser == null)
            {
                return BadRequest("No user was passed.");
            }

            if (updatedUser.UserId != null)
            {
                if (!await _userRepository.UserExistsAsync(updatedUser.UserId.Value))
                {
                    return NotFound($"No user with the Id of {updatedUser.UserId.Value} was found.");
                }
            }
            else 
            {
                return BadRequest(ModelState);
            }

            if (userId != updatedUser.UserId) 
            {
                return BadRequest("Route userId doesn't match body userId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedUser = _mapper.Map<User>(updatedUser);

            if (!await _userRepository.UpdateUserAsync(mappedUser)) 
            {
                ModelState.AddModelError("", "Something went wrong updating the user");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId) 
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"No user with the Id of {userId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (!await _userRepository.DeleteUserAsync(userId)) 
            {
                ModelState.AddModelError("", "Something went wrong deleting the user");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("{userId}/Password")]
        public async Task<IActionResult> UpdateUserPassword(int userId, [FromBody] UserPassword userPassword) 
        {
            if (userPassword == null) 
            {
                return BadRequest("No Password was passed");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (!await _userRepository.UserExistsAsync(userId)) 
            {
                return NotFound($"No user with the id of {userId} was found");
            }

            if (userId != userPassword.UserId)
            {
                return BadRequest($"Route userId does not match userId in the body.");
            }

            if (!await _userRepository.UpdateUserPasswordAsync(userPassword)) 
            {
                ModelState.AddModelError("", "Something went wrong updating the Password");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetAllUsersThatStartWithLetter([FromQuery] string letter)
        {
            if (letter == null || letter == "")
            {
                return BadRequest("A letter was not passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedUsers = _mapper.Map<List<UserDto>>(await _userRepository.GetAllUsersThatStartWithLetterAsync(letter));

            if (mappedUsers == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the Users.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedUsers);
        }

    }
}
