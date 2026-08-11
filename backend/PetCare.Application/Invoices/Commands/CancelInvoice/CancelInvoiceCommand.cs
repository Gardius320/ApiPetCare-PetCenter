using MediatR;

namespace PetCare.Application.Invoices.Commands.CancelInvoice
{
    public record CancelInvoiceCommand(Guid InvoiceId) : IRequest<Unit>;
}
