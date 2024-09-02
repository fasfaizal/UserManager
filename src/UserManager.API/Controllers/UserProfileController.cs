using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManager.Common.Interfaces.Services;

namespace UserManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UserProfileController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        /// Retrieves the profile details of the currently authenticated user.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the user's profile details.
        /// </returns>
        /// <response code="200">Returns the user's profile details.</response>
        /// <response code="401">Returns status code 401 if the user is not authenticated.</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userProfileDetails = await _usersService.GetUserProfileDetails(userId);
            return Ok(userProfileDetails);
        }
    }
}
