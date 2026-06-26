using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Appointments.Commands.CreateAppointment;
using PetCare.Application.Appointments.Commands.DeleteAppointment;
using PetCare.Application.Appointments.Commands.UpdateAppointment;
using PetCare.Application.Appointments.Queries.GetAllAppointments;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAll")]
    [Authorize(Roles = "Admin,Veterinario,Auxiliar")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new GetAllAppointmentsQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search
        });

        return Ok(ApiResponse<PaginatedAppointmentsResult?>.Success(result));
    }

    [HttpPost("Crear")]
    [Authorize(Roles = "Admin,Veterinario,Auxiliar")]
    public async Task<IActionResult> Crear([FromBody] CreateAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<int?>.Success(result));
    }

    [HttpDelete("Eliminar/{id}")]
    [Authorize(Roles = "Admin,Veterinario")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var result = await _mediator.Send(new DeleteAppointmentCommand { Id = id });
        return Ok(ApiResponse<bool>.Success(true, "Cita cancelada exitosamente"));
    }

    [HttpPut("Actualizar/{id}")]
    [Authorize(Roles = "Admin,Veterinario")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateAppointmentCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<int?>.Success(result));
    }
}