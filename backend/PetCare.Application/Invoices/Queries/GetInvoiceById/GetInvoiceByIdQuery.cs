using MediatR;
using PetCare.Application.Invoices.DTOs;

namespace PetCare.Application.Invoices.Queries.GetInvoiceById
{
    public record GetInvoiceByIdQuery(Guid Id) : IRequest<InvoiceDetailDto?>;
}
