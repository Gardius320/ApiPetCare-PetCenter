using MediatR;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Invoices.Commands.PayInvoice
{
    public class PayInvoiceCommandHandler : IRequestHandler<PayInvoiceCommand, Unit>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public PayInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Unit> Handle(PayInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice is null)
                throw new KeyNotFoundException("La factura no existe.");

            if (invoice.Status == InvoiceStatus.Cancelled)
                throw new InvalidOperationException("No se puede pagar una factura anulada.");

            if (invoice.Status == InvoiceStatus.Paid)
                throw new InvalidOperationException("La factura ya está pagada.");

            invoice.Status = InvoiceStatus.Paid;
            invoice.PaymentMethod = request.PaymentMethod;
            invoice.PaymentReference = request.PaymentReference;
            invoice.PaymentDate = DateTime.UtcNow;

            await _invoiceRepository.UpdateAsync(invoice);

            return Unit.Value;
        }
    }
}