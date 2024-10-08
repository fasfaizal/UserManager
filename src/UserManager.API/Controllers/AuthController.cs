﻿using Microsoft.AspNetCore.Mvc;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;

namespace UserManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;

        public AuthController(IUsersService usersService, IAuthService authService)
        {
            _usersService = usersService;
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user by creating a user.
        /// </summary>
        /// <param name="createUserRequest">An object containing the details of the user to be created.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the registration process.
        /// </returns>
        /// <response code="200">Indicates that the user is successfully cretaed.</response>
        /// <response code="400">Returns status code 400 if the request is invalid.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequest createUserRequest)
        {
            await _usersService.CreateAsync(createUserRequest);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="loginRequest">An object containing the user's login credentials (username and password).</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a JSON object with the JWT token if authentication is successful.
        /// </returns>
        /// <response code="200">Indicates that the login request was successful.</response>
        /// <response code="400">Returns status code 400 if the request is invalid.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var token = await _authService.AuthenticateAsync(loginRequest);
            return Ok(new { token });
        }
    }
}
