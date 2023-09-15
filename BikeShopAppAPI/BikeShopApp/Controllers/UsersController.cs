using AutoMapper;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using BikeShopApp.Core.Identity;
using BikeShopApp.Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace BikeShopApp.WebAPI.Controllers
{
    public class UsersController : CustomControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Users Controller Constructor.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users in the database.
        /// </summary>
        [HttpGet]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> GetUsers() 
        {
            List<UserDto> userDtos = _mapper.Map<List<UserDto>>(await _userRepository.GetUsersAsync());

            if (userDtos == null) 
            {
                return Problem(detail: "Something went wrong while getting the users.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(userDtos);
        }

        /// <summary>
        /// Get the user with the passed Id.
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId)) 
            {
                return Problem(detail: $"No user with the Id of {userId} was found.", statusCode: 404, title: "Not Found");
            }

            UserDto userDto = _mapper.Map<UserDto>(await _userRepository.GetUserByIdAsync(userId));

            if (userDto == null) 
            {
                return Problem(detail: "Something went wrong while getting the user.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(userDto);
        }

        /// <summary>
        /// Update an existing user based on the passed userDto.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDto"></param>
        [HttpPut("{userId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> UpdateUser(int userId, UserDto userDto) 
        {
            if (userDto == null)
            {
                return Problem(detail: "No user was passed.", statusCode: 400, title: "Bad Request");
            }

            if (userDto.UserId != null)
            {
                if (!await _userRepository.UserExistsAsync(userDto.UserId.Value))
                {
                    return Problem(detail: $"No user with the Id of {userDto.UserId.Value} was found.", statusCode: 404, title: "Not Found");
                }
            }
            else 
            {
                return Problem(detail: "No user id was passed.", statusCode: 400, title: "Bad Request");
            }

            if (userId != userDto.UserId) 
            {
                return Problem(detail: "Route userId doesn't match body userId.", statusCode: 400, title: "Bad Request");
            }

            if (!await _userRepository.UpdateUserAsync(userDto)) 
            {
                return Problem(detail: "Something went wrong while updating the user.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Delete an existing user with the passed Id.
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("{userId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> DeleteUser(int userId) 
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return Problem(detail: $"No user with the Id of {userId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _userRepository.DeleteUserAsync(userId)) 
            {
                return Problem(detail: "Something went wrong while deleting the user.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Update an existing user's password based on the passed userPasswordDto.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userPassword"></param>
        [HttpPut("{userId}/Password")]
        public async Task<IActionResult> UpdateUserPassword(int userId, [FromBody] UserPasswordDto userPassword)
        {
            if (userPassword == null)
            {
                return Problem(detail: "No password was passed.", statusCode: 400, title: "Bad Request");
            }

            if (!await _userRepository.UserExistsAsync(userId))
            {
                return Problem(detail: $"No user with the Id of {userId} was found.", statusCode: 404, title: "Not Found");
            }

            if (userId != userPassword.UserId)
            {
                return Problem(detail: "Route userId doesn't match body userId.", statusCode: 400, title: "Bad Request");
            }

            var result = await _userRepository.UpdateUserPasswordAsync(userPassword);

            if (result == null)
            {
                return Problem(detail: "Something went wrong while updating the password.", statusCode: 500, title: "Internal Server Error");
            }
            else if (result.Succeeded == false)
            {
                var errorString = "";

                var errors = result.Errors.Select(e => e.Description).ToList();

                for (int i = 0; i < errors.Count; i++)
                {
                    errorString += errors[i];

                    if (i != errors.Count - 1) 
                    {
                        errorString += " | ";
                    }
                }

                return Problem(detail: $"Invalid Password: {errorString}", statusCode: 400, title: "Bad Request");
            }

            return NoContent();
        }

        /// <summary>
        /// Get all users whose name starts with the passed letter.
        /// </summary>
        /// <param name="letter"></param>
        [HttpGet("name")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> GetAllUsersThatStartWithLetter([FromQuery] string letter)
        {
            if (letter == null || letter == "")
            {
                return Problem(detail: "A letter was not passed.", statusCode: 400, title: "Bad Request");
            }

            List<UserDto> userDtos = _mapper.Map<List<UserDto>>(await _userRepository.GetAllUsersThatStartWithLetterAsync(letter));

            if (userDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the Users.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(userDtos);
        }
    }
}
