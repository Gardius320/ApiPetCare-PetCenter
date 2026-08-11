using MediatR;
using PetCare.Application.Invoices.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Invoices.Queries.GetAllInvoices
{
    public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, List<InvoiceDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetAllInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetAllAsync(request.From, request.To, request.Status);

            return invoices.Select(invoice => new InvoiceDto(
                invoice.Id, invoice.InvoiceNumber, invoice.OwnerId, invoice.AppointmentId,
                invoice.IssueDate, invoice.Status, invoice.Subtotal, invoice.Tax, invoice.Total,
                invoice.PaymentMethod, invoice.PaymentReference, invoice.PaymentDate
            )).ToList();
        }
    }
}
