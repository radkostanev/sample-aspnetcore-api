using SampleNetCoreApi.Api.Auth.Contracts;
using SampleNetCoreApi.Api.Helpers;
using SampleNetCoreApi.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

namespace SampleNetCoreApi.Api.v1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService authService;

        public UsersController(IAuthService authService)
        {
            this.authService = authService;
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            try
            {
                var response = this.authService.Register(model);
                return Created(nameof(Register), response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = this.authService.Authenticate(model);

            if (response == null)
            {
                return Unauthorized(new { message = "Username and/or password is incorrect" });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var users = this.authService.GetAll();
            return Ok(users);
        }
    }
}
