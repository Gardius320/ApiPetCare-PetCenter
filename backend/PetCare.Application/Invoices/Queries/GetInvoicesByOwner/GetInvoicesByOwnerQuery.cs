using MediatR;
using PetCare.Application.Invoices.DTOs;

namespace PetCare.Application.Invoices.Queries.GetInvoicesByOwner
{
    public record GetInvoicesByOwnerQuery(int OwnerId) : IRequest<List<InvoiceDto>>;
}
