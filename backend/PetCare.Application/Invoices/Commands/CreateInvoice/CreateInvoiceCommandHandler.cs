using MediatR;
using Microsoft.Extensions.Options;
using PetCare.Application.Common.Settings;
using PetCare.Application.Invoices.DTOs;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ISupplyRepository _supplyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly decimal _taxRate;

        public CreateInvoiceCommandHandler(
            IInvoiceRepository invoiceRepository,
            IOwnerRepository ownerRepository,
            IAppointmentRepository appointmentRepository,
            ISupplyRepository supplyRepository,
            IUnitOfWork unitOfWork,
            IOptions<InvoiceSettings> invoiceSettings)
        {
            _invoiceRepository = invoiceRepository;
            _ownerRepository = ownerRepository;
            _appointmentRepository = appointmentRepository;
            _supplyRepository = supplyRepository;
            _unitOfWork = unitOfWork;
            _taxRate = invoiceSettings.Value.TaxRate;
        }

        public async Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new KeyNotFoundException("El dueño especificado no existe.");

            if (request.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId.Value);
                if (appointment is null)
                    throw new KeyNotFoundException("La cita especificada no existe.");

                if (appointment.OwnerId != request.OwnerId)
                    throw new InvalidOperationException("La cita no pertenece al dueño indicado.");

                if (appointment.State.StateName != "Completada")
                    throw new InvalidOperationException("Solo se pueden facturar citas en estado Completada.");

                var existingInvoice = await _invoiceRepository.GetByAppointmentIdAsync(request.AppointmentId.Value);
                if (existingInvoice is not null)
                    throw new InvalidOperationException("Esta cita ya tiene una factura asociada.");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var invoiceItems = new List<InvoiceItem>();

                foreach (var itemDto in request.Items)
                {
                    if (itemDto.ItemType == InvoiceItemType.Supply)
                    {
                        if (itemDto.SupplyId is null)
                            throw new InvalidOperationException("Un ítem de tipo Supply requiere SupplyId.");

                        var supply = await _supplyRepository.GetByIdAsync(itemDto.SupplyId.Value);
                        if (supply is null)
                            throw new KeyNotFoundException($"El insumo {itemDto.SupplyId} no existe.");

                        if (supply.CurrentStock < itemDto.Quantity)
                            throw new InvalidOperationException(
                                $"Stock insuficiente para '{supply.Name}'. Disponible: {supply.CurrentStock}, solicitado: {itemDto.Quantity}.");

                        supply.CurrentStock -= itemDto.Quantity;
                        await _supplyRepository.UpdateSupply(supply);
                    }

                    invoiceItems.Add(new InvoiceItem
                    {
                        ItemType = itemDto.ItemType,
                        Description = itemDto.Description,
                        SupplyId = itemDto.SupplyId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        LineTotal = itemDto.Quantity * itemDto.UnitPrice
                    });
                }

                var subtotal = invoiceItems.Sum(i => i.LineTotal);
                var tax = Math.Round(subtotal * _taxRate, 2);
                var total = subtotal + tax;

                var lastNumber = await _invoiceRepository.GetLastInvoiceNumberAsync();
                var nextNumber = GenerateNextInvoiceNumber(lastNumber);

                var invoice = new Invoice
                {
                    InvoiceNumber = nextNumber,
                    OwnerId = request.OwnerId,
                    AppointmentId = request.AppointmentId,
                    IssueDate = DateTime.UtcNow,
                    Status = InvoiceStatus.Pending,
                    Subtotal = subtotal,
                    Tax = tax,
                    Total = total,
                    CreatedByUserId = request.CreatedByUserId,
                    Items = invoiceItems
                };

                foreach (var item in invoiceItems)
                    item.InvoiceId = invoice.Id;

                await _invoiceRepository.AddAsync(invoice);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return new InvoiceDto(
                    invoice.Id, invoice.InvoiceNumber, invoice.OwnerId, invoice.AppointmentId,
                    invoice.IssueDate, invoice.Status, invoice.Subtotal, invoice.Tax, invoice.Total,
                     invoice.PaymentMethod, invoice.PaymentReference, invoice.PaymentDate
                );
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private static string GenerateNextInvoiceNumber(string lastNumber)
        {
            var parts = lastNumber.Split('-');
            var currentNumber = int.Parse(parts[1]);
            return $"FAC-{(currentNumber + 1):D4}";
        }
    }
}
