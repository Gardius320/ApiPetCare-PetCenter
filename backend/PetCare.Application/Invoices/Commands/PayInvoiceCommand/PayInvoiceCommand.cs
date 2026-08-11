using MediatR;
using PetCare.Domain.Enums;

namespace PetCare.Application.Invoices.Commands.PayInvoice
{
    public record PayInvoiceCommand(Guid InvoiceId, PaymentMethod PaymentMethod, string? PaymentReference) : IRequest<Unit>;
}