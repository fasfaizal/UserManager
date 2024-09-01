using Microsoft.AspNetCore.Mvc;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;

namespace UserManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public AuthController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        /// Registers a new user by creating a user.
        /// </summary>
        /// <param name="createUserRequest">An object containing the details of the user to be created.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the registration process.
        /// On success, returns a status code of 201 (Created).
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequest createUserRequest)
        {
            await _usersService.CreateAsync(createUserRequest);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
