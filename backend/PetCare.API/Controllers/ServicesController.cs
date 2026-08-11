using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Services.Commands.Create;
using PetCare.Application.Services.Queries.GetAllServices;
using PetCare.Application.Services.Queries.GetServiceById;

namespace PetCare.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllServicesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetServiceByIdQuery { Id = id });
            return Ok(result);
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