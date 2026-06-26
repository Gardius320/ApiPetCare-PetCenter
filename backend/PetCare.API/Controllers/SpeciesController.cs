using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.Species.Commands.CreateSpecies;
using PetCare.Application.Species.Commands.DeleteSpecies;
using PetCare.Application.Species.Queries.GetAllSpecies;
using PetCare.Domain.DTOs;

namespace PetCare.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SpeciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpeciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var species = await _mediator.Send(new GetAllSpeciesQuery());
            return Ok(ApiResponse<List<GetSpeciesDTO>>.Success(species));
        }

        [HttpPost("Crear")]
        [Authorize(Roles = "Admin,Veterinario,Auxiliar")]
        public async Task<IActionResult> Crear([FromBody] CreateSpeciesCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<int?>.Success(id));
        }

        [HttpDelete("Eliminar/{id}")]
        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _mediator.Send(new DeleteSpeciesCommand { Id = id });

            return Ok(ApiResponse<string>.Success(result, "Especie eliminada exitosamente"));
        }
    }
}
