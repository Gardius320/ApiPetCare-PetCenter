using Moq;
using PetCare.Application.Supplies.Commands.UpdateSupply;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Supplies
{
    public class UpdateSupplyCommandHandlerTests
    {
        private readonly Mock<ISupplyRepository> _supplyRepository;
        private readonly UpdateSupplyCommandHandler _handler;

        public UpdateSupplyCommandHandlerTests()
        {
            _supplyRepository = new Mock<ISupplyRepository>();
            _handler = new UpdateSupplyCommandHandler(_supplyRepository.Object);
        }

        private static UpdateSupplyCommand BuildCommand() => new()
        {
            Id = 1,
            Name = "Jeringas 5ml",
            Unit = "unidad",
            CurrentStock = 80,
            MinimumStock = 20,
            IsActive = true,
            SupplyCategoryId = 1,
            SupplyType = SupplyType.Clinical
        };

        [Fact]
        public async Task Handle_SupplyDoesNotExist_ReturnsFailure()
        {
            _supplyRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Domain.Models.Supply?)null);

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _supplyRepository.Verify(r => r.UpdateSupply(It.IsAny<Domain.Models.Supply>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryFailsToUpdate_ReturnsFailure()
        {
            // A diferencia de Update de Owner/Pet, acá el handler SÍ revisa
            // el resultado de UpdateSupply antes de responder.
            _supplyRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Domain.Models.Supply { Id = 1, Name = "Jeringas 5ml" });
            _supplyRepository
                .Setup(r => r.UpdateSupply(It.IsAny<Domain.Models.Supply>()))
                .ReturnsAsync((Domain.Models.Supply?)null);

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ValidSupply_UpdatesSuccessfully()
        {
            var existing = new Domain.Models.Supply { Id = 1, Name = "Jeringas 5ml" };

            _supplyRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);
            _supplyRepository
                .Setup(r => r.UpdateSupply(It.IsAny<Domain.Models.Supply>()))
                .ReturnsAsync((Domain.Models.Supply s) => s);

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(80, result.Data!.CurrentStock);
            _supplyRepository.Verify(r => r.UpdateSupply(It.Is<Domain.Models.Supply>(s =>
                s.CurrentStock == 80 && s.MinimumStock == 20)), Times.Once);
        }
    }
}
