using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Appointments.Commands.BookOnline;
using PetCare.Application.Appointments.Commands.CreateAppointment;
using PetCare.Application.Appointments.Commands.DeleteAppointment;
using PetCare.Application.Appointments.Commands.UpdateAppointment;
using PetCare.Application.Appointments.Queries.GetAllAppointments;
using PetCare.Application.Appointments.Queries.GetBillableAppointments;
using PetCare.Application.Appointments.Commands.ChangeAppointmentState;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class AppointmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAll")]
    [Authorize(Roles = "Admin,Veterinarian,Assistant")]
    public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] int? petId = null)
    {
        var result = await _mediator.Send(new GetAllAppointmentsQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            PetId = petId
        });

        return Ok(ApiResponse<PaginatedAppointmentsResult?>.Success(result));
    }

    [HttpPost("Create")]
    [Authorize(Roles = "Admin,Veterinarian,Assistant")]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
    {
        var result = await _mediator.Send(command);

        if (result is null)
            return NotFound(ApiResponse<int?>.Failure("El propietario especificado no existe."));

        return Ok(ApiResponse<int?>.Success(result));
    }

    [HttpDelete("Delete/{id}")]
    [Authorize(Roles = "Admin,Veterinarian")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteAppointmentCommand { Id = id });

        if (result is null)
            return NotFound(ApiResponse<bool>.Failure("La cita no existe."));

        return Ok(ApiResponse<bool>.Success(true, "Cita cancelada exitosamente"));
    }

    [HttpPut("Update/{id}")]
    [Authorize(Roles = "Admin,Veterinarian")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (result is null)
            return NotFound(ApiResponse<int?>.Failure("La cita no existe."));

        return Ok(ApiResponse<int?>.Success(result));
    }
    [HttpPatch("ChangeState/{id}")]
    [Authorize(Roles = "Admin,Veterinarian,Assistant")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeAppointmentStateCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound(ApiResponse<bool>.Failure("La cita o el estado no existen."));

        return Ok(ApiResponse<bool>.Success(true, "Estado de la cita actualizado"));
    }

    [HttpPost("BookOnline")]
    [AllowAnonymous]
    public async Task<IActionResult> BookOnline([FromBody] BookOnlineCommand command)
    {
        var result = await _mediator.Send(command);

        if (result is null)
            return BadRequest(ApiResponse<int?>.Failure("No fue posible registrar la reserva."));

        return Ok(ApiResponse<int?>.Success(result, "Solicitud de cita recibida. Te contactaremos para confirmarla."));
    }

    [HttpGet("Billable")]
    [Authorize(Roles = "Admin,Veterinarian,Assistant")]
    public async Task<IActionResult> GetBillable([FromQuery] int ownerId)
    {
        var result = await _mediator.Send(new GetBillableAppointmentsQuery(ownerId));
        return Ok(ApiResponse<List<BillableAppointmentDto>>.Success(result));
    }
}