using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.Owners.Commands.CreateOwner;
using PetCare.Application.Owners.Commands.DeleteOwner;
using PetCare.Application.Owners.Commands.UpdateOwner;
using PetCare.Application.Owners.Queries.GetAllOwners;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OwnersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    [Authorize(Roles = "Admin,Assistant")]
    public async Task<IActionResult> Create([FromBody] CreateOwnerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<int?>.Success(result));
    }

    [HttpGet("GetAll")]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new GetAllOwnersQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search
        });

        return Ok(ApiResponse<PaginatedOwnersResult?>.Success(result));
    }

    [HttpPut("Update/{id}")]
    [Authorize(Roles = "Admin,Assistant")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOwnerCommand command)
    {
        command.OwnerId = id;
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<int?>.Success(result));
    }

    [HttpDelete("Delete/{id}")]
    [Authorize(Roles = "Admin,Assistant")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteOwnerCommand { Id = id });
        return Ok(result);
    }
}