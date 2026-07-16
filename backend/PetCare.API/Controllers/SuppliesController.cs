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

    [HttpPost("Crear")]
    [Authorize(Roles = "Admin,Auxiliar")]
    public async Task<IActionResult> Crear([FromBody] CreateSupplyCommand command)
    {
        var resultado = await _mediator.Send(command);
        return Ok(resultado);
    }

    [HttpPut("Actualizar/{id}")]
    [Authorize(Roles = "Admin,Auxiliar")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateSupplyCommand command)
    {
        command.Id = id;
        var resultado = await _mediator.Send(command);
        return Ok(resultado);
    }

    [HttpPut("CambiarEstado/{id}")]
    [Authorize(Roles = "Admin,Auxiliar")]
    public async Task<IActionResult> CambiarEstado(int id)
    {
        var resultado = await _mediator.Send(new ToggleSupplyStatusCommand { Id = id });
        return Ok(resultado);
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