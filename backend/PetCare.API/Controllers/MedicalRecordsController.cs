using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.MedicalRecord.Commands.Create;
using PetCare.Application.MedicalRecord.Queries.GetMedicalRecordById;
using PetCare.Application.MedicalRecord.Queries.GetAllMedicalRecord;
using PetCare.Application.MedicalRecord.Queries.GetMedicalRecordsByPetId;

namespace PetCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Veterinarian,Assistant")]
        public async Task<IActionResult> Create([FromBody] CreateCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMedicalRecordQuery { Id = id });
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("pet/{petId}")]
        [Authorize]
        public async Task<IActionResult> GetByPetId(int petId)
        {
            var result = await _mediator.Send(new GetMedicalRecordsByPetIdQuery { PetId = petId });
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Veterinarian,Assistant")]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? vetId)
        {
            var result = await _mediator.Send(new GetAllMedicalRecordQuery
            {
                From = from,
                To = to,
                VetId = vetId
            });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Veterinarian,Assistant")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommand command)
        {
            if (id != command.Id)
                return BadRequest("El Id de la ruta no coincide con el Id del cuerpo.");

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Veterinarian,Assistant")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCommand { Id = id });
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}