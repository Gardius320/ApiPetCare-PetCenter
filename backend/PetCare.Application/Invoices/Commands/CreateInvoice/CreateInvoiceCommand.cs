using MediatR;
using PetCare.Application.Invoices.DTOs;
using PetCare.Domain.Enums;

namespace PetCare.Application.Invoices.Commands.CreateInvoice
{
    public record CreateInvoiceCommand(
        int OwnerId,
        int? AppointmentId,
        string CreatedByUserId,
        List<CreateInvoiceItemDto> Items
    ) : IRequest<InvoiceDto>;

    public record CreateInvoiceItemDto(
        InvoiceItemType ItemType,
        string Description,
        int? SupplyId,
        int Quantity,
        decimal UnitPrice
    );
}
