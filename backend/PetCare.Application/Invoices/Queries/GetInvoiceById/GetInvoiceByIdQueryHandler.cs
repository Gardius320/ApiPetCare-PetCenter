using MediatR;
using PetCare.Application.Invoices.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDetailDto?>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoiceByIdQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<InvoiceDetailDto?> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByIdWithItemsAsync(request.Id);
            if (invoice is null) return null;

              var invoiceDto = new InvoiceDto(
               invoice.Id, invoice.InvoiceNumber, invoice.OwnerId, invoice.AppointmentId,
               invoice.IssueDate, invoice.Status, invoice.Subtotal, invoice.Tax, invoice.Total,
               invoice.PaymentMethod, invoice.PaymentReference, invoice.PaymentDate

            );

            var itemsDto = invoice.Items.Select(i => new InvoiceItemDto(
                i.Id, i.ItemType, i.Description, i.SupplyId, i.Quantity, i.UnitPrice, i.LineTotal
            )).ToList();

            return new InvoiceDetailDto(invoiceDto, itemsDto);
        }
    }
}
