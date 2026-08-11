using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Application.Invoices.Commands.CancelInvoice;
using PetCare.Application.Invoices.Commands.CreateInvoice;
using PetCare.Application.Invoices.Commands.PayInvoice;
using PetCare.Application.Invoices.DTOs;
using PetCare.Application.Invoices.Queries.GetAllInvoices;
using PetCare.Application.Invoices.Queries.GetInvoiceById;
using PetCare.Application.Invoices.Queries.GetInvoicesByOwner;
using PetCare.Domain.Enums;
using System.Security.Claims;


namespace PetCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Veterinarian,Assistant")]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest request)
        {
            var userId = User.FindFirstValue("id");
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var command = new CreateInvoiceCommand(request.OwnerId, request.AppointmentId, userId, request.Items);

            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Id },
                    ApiResponse<InvoiceDto>.Success(result, "Factura creada correctamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<InvoiceDto>.Failure(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<InvoiceDto>.Failure(ex.Message));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetInvoiceByIdQuery(id));
            if (result is null)
                return NotFound(ApiResponse<InvoiceDetailDto>.Failure("La factura no existe."));

            return Ok(ApiResponse<InvoiceDetailDto>.Success(result));
        }

        [HttpGet("owner/{ownerId:int}")]
        public async Task<IActionResult> GetByOwner(int ownerId)
        {
            var result = await _mediator.Send(new GetInvoicesByOwnerQuery(ownerId));
            return Ok(ApiResponse<List<InvoiceDto>>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] InvoiceStatus? status)
        {
            var result = await _mediator.Send(new GetAllInvoicesQuery(from, to, status));
            return Ok(ApiResponse<List<InvoiceDto>>.Success(result));
        }

        [HttpPut("{id:guid}/cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                await _mediator.Send(new CancelInvoiceCommand(id));
                return Ok(ApiResponse<object>.Success(null, "Factura anulada correctamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Failure(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
        }

        [HttpPut("{id:guid}/pay")]
        [Authorize(Roles = "Admin,Assistant")]
        public async Task<IActionResult> Pay(Guid id, [FromBody] PayInvoiceRequest request)
        {
            try
            {
                await _mediator.Send(new PayInvoiceCommand(id, request.PaymentMethod, request.PaymentReference));
                return Ok(ApiResponse<object>.Success(null, "Factura marcada como pagada"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Failure(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
        }
    }

    public record CreateInvoiceRequest(int OwnerId, int? AppointmentId, List<CreateInvoiceItemDto> Items);
    public record PayInvoiceRequest(PaymentMethod PaymentMethod, string? PaymentReference);
}