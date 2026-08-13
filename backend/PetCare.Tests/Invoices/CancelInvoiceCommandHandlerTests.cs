using Moq;
using PetCare.Application.Invoices.Commands.CancelInvoice;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Invoices
{
    public class CancelInvoiceCommandHandlerTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
        private readonly Mock<ISupplyRepository> _supplyRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CancelInvoiceCommandHandler _handler;

        public CancelInvoiceCommandHandlerTests()
        {
            _handler = new CancelInvoiceCommandHandler(
                _invoiceRepository.Object,
                _supplyRepository.Object,
                _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_InvoiceDoesNotExist_ThrowsKeyNotFoundException()
        {
            var invoiceId = Guid.NewGuid();
            _invoiceRepository.Setup(r => r.GetByIdWithItemsAsync(invoiceId)).ReturnsAsync((Invoice?)null);

            var command = new CancelInvoiceCommand(invoiceId);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvoiceAlreadyCancelled_ThrowsInvalidOperationException()
        {
            var invoiceId = Guid.NewGuid();
            _invoiceRepository.Setup(r => r.GetByIdWithItemsAsync(invoiceId))
                .ReturnsAsync(new Invoice { Id = invoiceId, Status = InvoiceStatus.Cancelled });

            var command = new CancelInvoiceCommand(invoiceId);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidInvoiceWithSupplyItems_RestoresStockAndCancels()
        {
            var invoiceId = Guid.NewGuid();
            var supply = new Supply { Id = 7, Name = "Vacuna", CurrentStock = 2 };

            var invoice = new Invoice
            {
                Id = invoiceId,
                Status = InvoiceStatus.Pending,
                Items = new List<InvoiceItem>
                {
                    new() { ItemType = InvoiceItemType.Supply, SupplyId = 7, Quantity = 3 },
                    new() { ItemType = InvoiceItemType.Service, Quantity = 1 }
                }
            };

            _invoiceRepository.Setup(r => r.GetByIdWithItemsAsync(invoiceId)).ReturnsAsync(invoice);
            _supplyRepository.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(supply);

            var command = new CancelInvoiceCommand(invoiceId);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(5m, supply.CurrentStock); // 2 + 3 restaurado
            Assert.Equal(InvoiceStatus.Cancelled, invoice.Status);
            _supplyRepository.Verify(r => r.UpdateSupply(supply), Times.Once);
            _invoiceRepository.Verify(r => r.UpdateAsync(invoice), Times.Once);
            _unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UpdateAsyncThrows_RollsBackTransactionAndRethrows()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Invoice { Id = invoiceId, Status = InvoiceStatus.Pending };

            _invoiceRepository.Setup(r => r.GetByIdWithItemsAsync(invoiceId)).ReturnsAsync(invoice);
            _invoiceRepository.Setup(r => r.UpdateAsync(invoice)).ThrowsAsync(new InvalidOperationException("db down"));

            var command = new CancelInvoiceCommand(invoiceId);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
