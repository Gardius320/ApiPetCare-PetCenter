using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.SupplyCategories.Commands.Create;
using PetCare.Application.SupplyCategories.Queries.GetAllCategories;
using PetCare.Domain.DTOs;

namespace PetCare.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SupplyCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplyCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool onlyActive = true)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                OnlyActive = onlyActive
            });

            return Ok(ApiResponse<PaginatedCategoriesResult>.Success(result));
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin,Assistant")]
        public async Task<IActionResult> Create([FromBody] CreateCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin,Assistant")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}