using Moq;
using PetCare.Application.Invoices.Commands.PayInvoice;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Invoices
{
    public class PayInvoiceCommandHandlerTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
        private readonly PayInvoiceCommandHandler _handler;

        public PayInvoiceCommandHandlerTests()
        {
            _handler = new PayInvoiceCommandHandler(_invoiceRepository.Object);
        }

        [Fact]
        public async Task Handle_InvoiceDoesNotExist_ThrowsKeyNotFoundException()
        {
            var invoiceId = Guid.NewGuid();
            _invoiceRepository.Setup(r => r.GetByIdAsync(invoiceId)).ReturnsAsync((Invoice?)null);

            var command = new PayInvoiceCommand(invoiceId, PaymentMethod.Cash, null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvoiceIsCancelled_ThrowsInvalidOperationException()
        {
            var invoiceId = Guid.NewGuid();
            _invoiceRepository.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync(new Invoice { Id = invoiceId, Status = InvoiceStatus.Cancelled });

            var command = new PayInvoiceCommand(invoiceId, PaymentMethod.Cash, null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvoiceAlreadyPaid_ThrowsInvalidOperationException()
        {
            var invoiceId = Guid.NewGuid();
            _invoiceRepository.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync(new Invoice { Id = invoiceId, Status = InvoiceStatus.Paid });

            var command = new PayInvoiceCommand(invoiceId, PaymentMethod.Cash, null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_PendingInvoice_MarksAsPaidWithPaymentDetails()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Invoice { Id = invoiceId, Status = InvoiceStatus.Pending };

            _invoiceRepository.Setup(r => r.GetByIdAsync(invoiceId)).ReturnsAsync(invoice);

            var command = new PayInvoiceCommand(invoiceId, PaymentMethod.Transfer, "REF-123");

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(InvoiceStatus.Paid, invoice.Status);
            Assert.Equal(PaymentMethod.Transfer, invoice.PaymentMethod);
            Assert.Equal("REF-123", invoice.PaymentReference);
            Assert.NotNull(invoice.PaymentDate);
            _invoiceRepository.Verify(r => r.UpdateAsync(invoice), Times.Once);
        }
    }
}
