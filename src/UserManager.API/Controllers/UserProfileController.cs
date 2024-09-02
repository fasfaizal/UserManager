using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;

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

        /// <summary>
        /// Updates the profile details of the currently authenticated user.
        /// </summary>
        /// <param name="userDetails">The updated user profile details.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the update operation.
        /// </returns>
        /// <response code="200">Indicates that the user's profile details were successfully updated.</response>
        /// <response code="400">Returns status code 400 if the request is invalid.</response>
        /// <response code="401">Returns status code 401 if the user is not authenticated.</response>
        [HttpPut]
        public async Task<IActionResult> Put(UserDetailsRequest userDetails)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _usersService.UpdateUserProfileDetails(userId, userDetails);
            return Ok();
        }
    }
}
