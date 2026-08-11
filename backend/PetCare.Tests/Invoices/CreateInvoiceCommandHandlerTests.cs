using Microsoft.Extensions.Options;
using Moq;
using PetCare.Application.Common.Settings;
using PetCare.Application.Invoices.Commands.CreateInvoice;
using PetCare.Domain.Constants;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Invoices
{
    public class CreateInvoiceCommandHandlerTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
        private readonly Mock<IOwnerRepository> _ownerRepository = new();
        private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
        private readonly Mock<ISupplyRepository> _supplyRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CreateInvoiceCommandHandler _handler;

        public CreateInvoiceCommandHandlerTests()
        {
            var settings = Options.Create(new InvoiceSettings { TaxRate = 0.19m });
            _handler = new CreateInvoiceCommandHandler(
                _invoiceRepository.Object,
                _ownerRepository.Object,
                _appointmentRepository.Object,
                _supplyRepository.Object,
                _unitOfWork.Object,
                settings);

            _invoiceRepository.Setup(r => r.GetLastInvoiceNumberAsync()).ReturnsAsync("FAC-0001");
        }

        private static Owner MakeOwner(int id = 1) => new() { Id = id, OwnerName = "Jose", Email = "jose@petcare.com" };

        private static Appointment MakeAppointment(int id, int ownerId, string stateName) => new()
        {
            Id = id,
            OwnerId = ownerId,
            State = new State { IdState = 1, StateName = stateName },
            CreateAt = Array.Empty<byte>()
        };

        private static CreateInvoiceItemDto ServiceItem(decimal unitPrice = 100m, int quantity = 1) =>
            new(InvoiceItemType.Service, "Consulta general", null, quantity, unitPrice);

        [Fact]
        public async Task Handle_OwnerDoesNotExist_ThrowsKeyNotFoundException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Owner?)null);

            var command = new CreateInvoiceCommand(1, null, "user-1", new List<CreateInvoiceItemDto> { ServiceItem() });

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_AppointmentDoesNotExist_ThrowsKeyNotFoundException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner());
            _appointmentRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Appointment?)null);

            var command = new CreateInvoiceCommand(1, 99, "user-1", new List<CreateInvoiceItemDto> { ServiceItem() });

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_AppointmentBelongsToDifferentOwner_ThrowsInvalidOperationException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            _appointmentRepository.Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(MakeAppointment(5, ownerId: 2, AppointmentStateNames.Completed));

            var command = new CreateInvoiceCommand(1, 5, "user-1", new List<CreateInvoiceItemDto> { ServiceItem() });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_AppointmentNotCompleted_ThrowsInvalidOperationException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            _appointmentRepository.Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(MakeAppointment(5, ownerId: 1, AppointmentStateNames.Scheduled));

            var command = new CreateInvoiceCommand(1, 5, "user-1", new List<CreateInvoiceItemDto> { ServiceItem() });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_AppointmentAlreadyInvoiced_ThrowsInvalidOperationException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            _appointmentRepository.Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(MakeAppointment(5, ownerId: 1, AppointmentStateNames.Completed));
            _invoiceRepository.Setup(r => r.GetByAppointmentIdAsync(5))
                .ReturnsAsync(new Invoice { OwnerId = 1, AppointmentId = 5 });

            var command = new CreateInvoiceCommand(1, 5, "user-1", new List<CreateInvoiceItemDto> { ServiceItem() });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_SupplyItemWithoutSupplyId_ThrowsInvalidOperationException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            var items = new List<CreateInvoiceItemDto> { new(InvoiceItemType.Supply, "Vacuna", null, 1, 50m) };

            var command = new CreateInvoiceCommand(1, null, "user-1", items);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_SupplyDoesNotExist_ThrowsKeyNotFoundException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            _supplyRepository.Setup(r => r.GetByIdAsync(7)).ReturnsAsync((Supply?)null);
            var items = new List<CreateInvoiceItemDto> { new(InvoiceItemType.Supply, "Vacuna", 7, 1, 50m) };

            var command = new CreateInvoiceCommand(1, null, "user-1", items);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InsufficientStock_ThrowsInvalidOperationException()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            _supplyRepository.Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Supply { Id = 7, Name = "Vacuna antirrábica", CurrentStock = 2 });
            var items = new List<CreateInvoiceItemDto> { new(InvoiceItemType.Supply, "Vacuna", 7, 5, 50m) };

            var command = new CreateInvoiceCommand(1, null, "user-1", items);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidServiceItems_CalculatesSubtotalTaxAndTotal()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));

            var items = new List<CreateInvoiceItemDto>
            {
                ServiceItem(unitPrice: 100m, quantity: 1),
                ServiceItem(unitPrice: 50m, quantity: 2),
            };

            var command = new CreateInvoiceCommand(1, null, "user-1", items);

            var result = await _handler.Handle(command, CancellationToken.None);

            // subtotal = 100*1 + 50*2 = 200; tax = 200 * 0.19 = 38; total = 238
            Assert.Equal(200m, result.Subtotal);
            Assert.Equal(38m, result.Tax);
            Assert.Equal(238m, result.Total);
            Assert.Equal(InvoiceStatus.Pending, result.Status);
            Assert.Equal("FAC-0002", result.InvoiceNumber);

            _invoiceRepository.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidSupplyItem_DecrementsStockAndPersistsSupply()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            var supply = new Supply { Id = 7, Name = "Vacuna antirrábica", CurrentStock = 10 };
            _supplyRepository.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(supply);

            var items = new List<CreateInvoiceItemDto> { new(InvoiceItemType.Supply, "Vacuna", 7, 3, 20m) };
            var command = new CreateInvoiceCommand(1, null, "user-1", items);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(7m, supply.CurrentStock);
            _supplyRepository.Verify(r => r.UpdateSupply(supply), Times.Once);
        }

        [Fact]
        public async Task Handle_AddAsyncThrows_RollsBackTransactionAndRethrows()
        {
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeOwner(1));
            _invoiceRepository.Setup(r => r.AddAsync(It.IsAny<Invoice>())).ThrowsAsync(new InvalidOperationException("db down"));

            var command = new CreateInvoiceCommand(1, null, "user-1", new List<CreateInvoiceItemDto> { ServiceItem() });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
