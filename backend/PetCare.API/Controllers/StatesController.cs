using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.States.Commands.CreateStates;
using PetCare.Application.States.Commands.DeleteStates;
using PetCare.Application.States.Queries.GetAllStates;
using PetCare.Domain.DTOs;

namespace PetCare.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllStatesQuery());
            return Ok(ApiResponse<List<GetStatesDTO>>.Success(result));
        }

        [HttpPost("Crear")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear([FromBody] CreateStatesCommand command)
        {
            
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<int?>.Success(result, "Estado creado correctamente"));
        }

        [HttpDelete("Eliminar/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _mediator.Send(new DeleteStatesCommand { Id = id });
            return Ok(ApiResponse<int?>.Success(result));
        }
    }
}
