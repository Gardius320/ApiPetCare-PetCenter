using MediatR;
using PetCare.Application.Invoices.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Invoices.Queries.GetInvoicesByOwner
{
    public class GetInvoicesByOwnerQueryHandler : IRequestHandler<GetInvoicesByOwnerQuery, List<InvoiceDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoicesByOwnerQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<InvoiceDto>> Handle(GetInvoicesByOwnerQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetByOwnerIdAsync(request.OwnerId);

            return invoices.Select(invoice => new InvoiceDto(
                invoice.Id, invoice.InvoiceNumber, invoice.OwnerId, invoice.AppointmentId,
                invoice.IssueDate, invoice.Status, invoice.Subtotal, invoice.Tax, invoice.Total,
                invoice.PaymentMethod, invoice.PaymentReference, invoice.PaymentDate
            )).ToList();
        }
    }
}
