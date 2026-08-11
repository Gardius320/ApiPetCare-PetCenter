using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.Supplies.Commands.CreateSupply;
using PetCare.Application.Supplies.Queries.GetAllSupplies;
using PetCare.Application.Supplies.Commands.UpdateSupply;
using PetCare.Application.Supplies.Commands.ToggleSupplyStatus;
using PetCare.Application.Supplies.Queries.GetSupplyStats;
using PetCare.Domain.DTOs;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SuppliesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    [Authorize(Roles = "Admin,Assistant")]
    public async Task<IActionResult> Create([FromBody] CreateSupplyCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("Update/{id}")]
    [Authorize(Roles = "Admin,Assistant")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplyCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPut("ChangeState/{id}")]
    [Authorize(Roles = "Admin,Assistant")]
    public async Task<IActionResult> ChangeState(int id)
    {
        var result = await _mediator.Send(new ToggleSupplyStatusCommand { Id = id });

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("GetAll")]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] bool onlyActive = true)
    {
        var result = await _mediator.Send(new GetAllSuppliesQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            CategoryId = categoryId,
            OnlyActive = onlyActive
        });

        return Ok(ApiResponse<PaginatedSuppliesResult?>.Success(result));
    }

    [HttpGet("Stats")]
    [Authorize]
    public async Task<IActionResult> GetStats()
    {
        var result = await _mediator.Send(new GetSupplyStatsQuery());
        return Ok(ApiResponse<SupplyStatsDTO>.Success(result));
    }
}