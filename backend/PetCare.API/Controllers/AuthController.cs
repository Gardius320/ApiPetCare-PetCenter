using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Auth;
using PetCare.Application.Auth.Commands.Login;
using PetCare.Application.Auth.Commands.Logout;
using PetCare.Application.Auth.Commands.RefreshToken;
using PetCare.Application.Auth.Commands.Register;
using System.Security.Claims;

namespace PetCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _mediator.Send(new RegisterCommand
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            });

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _mediator.Send(new LoginCommand
            {
                Email = model.Email,
                Password = model.Password
            });

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto model)
        {
            var result = await _mediator.Send(new RefreshTokenCommand
            {
                RefreshToken = model.RefreshToken
            });

            return Ok(result);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
        {
            var userId = User.FindFirstValue("id");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _mediator.Send(new LogoutCommand
            {
                RefreshToken = dto.RefreshToken,
                UserId = userId
            });

            return Ok();
        }
    }
}
