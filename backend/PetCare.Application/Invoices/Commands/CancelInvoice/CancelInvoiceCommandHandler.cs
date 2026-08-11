using MediatR;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Invoices.Commands.CancelInvoice
{
    public class CancelInvoiceCommandHandler : IRequestHandler<CancelInvoiceCommand, Unit>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISupplyRepository _supplyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelInvoiceCommandHandler(
            IInvoiceRepository invoiceRepository,
            ISupplyRepository supplyRepository,
            IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _supplyRepository = supplyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByIdWithItemsAsync(request.InvoiceId);
            if (invoice is null)
                throw new KeyNotFoundException("La factura no existe.");

            if (invoice.Status == InvoiceStatus.Cancelled)
                throw new InvalidOperationException("La factura ya está anulada.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var item in invoice.Items.Where(i => i.ItemType == InvoiceItemType.Supply && i.SupplyId.HasValue))
                {
                    var supply = await _supplyRepository.GetByIdAsync(item.SupplyId!.Value);
                    if (supply is not null)
                    {
                        supply.CurrentStock += item.Quantity;
                        await _supplyRepository.UpdateSupply(supply);
                    }
                }

                invoice.Status = InvoiceStatus.Cancelled;
                await _invoiceRepository.UpdateAsync(invoice);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return Unit.Value;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
