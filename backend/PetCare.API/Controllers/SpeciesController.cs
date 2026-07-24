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

        [HttpPost("Create")]
        [Authorize(Roles = "Admin,Veterinarian,Assistant")]
        public async Task<IActionResult> Create([FromBody] CreateSpeciesCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<int?>.Success(id));
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin,Veterinarian")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSpeciesCommand { Id = id });

            return Ok(ApiResponse<string>.Success(result, "Especie eliminada exitosamente"));
        }
    }
}
