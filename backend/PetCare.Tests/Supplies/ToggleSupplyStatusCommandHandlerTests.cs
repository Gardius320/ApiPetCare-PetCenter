using Moq;
using PetCare.Application.Supplies.Commands.ToggleSupplyStatus;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Supplies
{
    public class ToggleSupplyStatusCommandHandlerTests
    {
        private readonly Mock<ISupplyRepository> _supplyRepository;
        private readonly ToggleSupplyStatusCommandHandler _handler;

        public ToggleSupplyStatusCommandHandlerTests()
        {
            _supplyRepository = new Mock<ISupplyRepository>();
            _handler = new ToggleSupplyStatusCommandHandler(_supplyRepository.Object);
        }

        [Fact]
        public async Task Handle_SupplyDoesNotExist_ReturnsFailure()
        {
            _supplyRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Domain.Models.Supply?)null);

            var result = await _handler.Handle(new ToggleSupplyStatusCommand { Id = 1 }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _supplyRepository.Verify(r => r.UpdateSupply(It.IsAny<Domain.Models.Supply>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryFailsToUpdate_ReturnsFailure()
        {
            _supplyRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Domain.Models.Supply { Id = 1, IsActive = true });
            _supplyRepository
                .Setup(r => r.UpdateSupply(It.IsAny<Domain.Models.Supply>()))
                .ReturnsAsync((Domain.Models.Supply?)null);

            var result = await _handler.Handle(new ToggleSupplyStatusCommand { Id = 1 }, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ActiveSupply_TogglesToInactive()
        {
            var existing = new Domain.Models.Supply { Id = 1, IsActive = true };

            _supplyRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);
            _supplyRepository
                .Setup(r => r.UpdateSupply(It.IsAny<Domain.Models.Supply>()))
                .ReturnsAsync((Domain.Models.Supply s) => s);

            var result = await _handler.Handle(new ToggleSupplyStatusCommand { Id = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.False(result.Data!.IsActive);
            Assert.Equal("Insumo desactivado correctamente", result.Message);
        }

        [Fact]
        public async Task Handle_InactiveSupply_TogglesToActive()
        {
            var existing = new Domain.Models.Supply { Id = 1, IsActive = false };

            _supplyRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);
            _supplyRepository
                .Setup(r => r.UpdateSupply(It.IsAny<Domain.Models.Supply>()))
                .ReturnsAsync((Domain.Models.Supply s) => s);

            var result = await _handler.Handle(new ToggleSupplyStatusCommand { Id = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.True(result.Data!.IsActive);
            Assert.Equal("Insumo activado correctamente", result.Message);
        }
    }
}
