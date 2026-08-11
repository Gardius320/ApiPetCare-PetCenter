using MediatR;
using PetCare.Application.Invoices.DTOs;
using PetCare.Domain.Enums;

namespace PetCare.Application.Invoices.Queries.GetAllInvoices
{
    public record GetAllInvoicesQuery(DateTime? From, DateTime? To, InvoiceStatus? Status) : IRequest<List<InvoiceDto>>;
}
