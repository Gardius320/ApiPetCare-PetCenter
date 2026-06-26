using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.Pets.Commands.ChangeStatePet;
using PetCare.Application.Pets.Commands.CreatePet;
using PetCare.Application.Pets.Commands.UpdatePet;
using PetCare.Application.Pets.Queries.GetAllPets;
using PetCare.Application.Pets.Queries.GetPetById;
using PetCare.Domain.DTOs;

namespace PetCare.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new GetAllPetsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search
            });

            return Ok(ApiResponse<PaginatedPetsResult?>.Success(result));
        }

        [HttpGet("GetById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPetByIdQuery { Id = id });
            return Ok(ApiResponse<GetPetDTO?>.Success(result));
        }

        [HttpPost("Crear")]
        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Crear([FromBody] CreatePetCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<int?>.Success(result));
        }

        [HttpPut("Actualizar/{id}")]
        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdatePetCommand command)
        {
            command.PetId = id;
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<int?>.Success(result));
        }

        [HttpPatch("CambiarEstado/{id}")]
        [Authorize]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] ChangeStatePetCommand command)
        {
            command.PetId = id;
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<int?>.Success(result));
        }
    }
}
