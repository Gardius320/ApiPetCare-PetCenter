using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.Users.Commands.ChangeUserRole;
using PetCare.Application.Users.Commands.CreateUsers;
using PetCare.Application.Users.Commands.DeleteUser;
using PetCare.Application.Users.Queries.GetAllUsers;
using PetCare.Application.Users.Queries.GetUsersByRole;
using PetCare.Domain.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Veterinarian,Assistant")]
    [HttpGet("ByRole/{role}")]
    public async Task<IActionResult> GetByRole(string role)
    {
        var result = await _mediator.Send(new GetUsersByRoleQuery { Role = role });
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var result = await _mediator.Send(new CreateUsersCommand
        {
            Email = dto.Email,
            Password = dto.Password,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role
        });

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("ChangeRole/{id}")]
    public async Task<IActionResult> ChangeRole(string id, [FromBody] ChangeRoleDto dto)
    {
        var result = await _mediator.Send(new ChangeUserRoleCommand
        {
            UserId = id,
            RoleName = dto.Role
        });

        if (!result.IsSuccess)
        {
            if (result.Message == "Usuario no encontrado")
                return NotFound(result);

            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { Id = id });

        if (!result.IsSuccess)
        {
            if (result.Message == "Usuario no encontrado")
                return NotFound(result);

            if (result.Message.StartsWith("No se puede eliminar"))
                return Conflict(result);

            return BadRequest(result);
        }

        return Ok(result);
    }
}
