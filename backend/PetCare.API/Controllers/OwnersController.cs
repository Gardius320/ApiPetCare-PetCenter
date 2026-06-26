using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.Owners.Commands.CreateOwner;
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

    [HttpPost("Crear")]
    [Authorize(Roles = "Admin,Auxiliar")]
    public async Task<IActionResult> Crear([FromBody] CreateOwnerCommand command)
    {
        var resultado = await _mediator.Send(command);
        return Ok(ApiResponse<int?>.Success(resultado));
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

    [HttpPut("Actualizar/{id}")]
    [Authorize(Roles = "Admin,Auxiliar")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateOwnerCommand command)
    {
        command.OwnerId = id;
        var resultado = await _mediator.Send(command);
        return Ok(ApiResponse<int?>.Success(resultado));
    }
}