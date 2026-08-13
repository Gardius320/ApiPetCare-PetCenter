using Moq;
using PetCare.Application.SupplyCategories.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.SupplyCategories
{
    public class UpdateCommandHandlerTests
    {
        private readonly Mock<ISupplyCategoryRepository> _supplyCategoryRepository;
        private readonly UpdateCommandHandler _handler;

        public UpdateCommandHandlerTests()
        {
            _supplyCategoryRepository = new Mock<ISupplyCategoryRepository>();
            _handler = new UpdateCommandHandler(_supplyCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_CategoryDoesNotExist_ReturnsFailure()
        {
            _supplyCategoryRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Domain.Models.SupplyCategory?)null);

            var command = new UpdateCommand { Id = 1, Name = "Medicamentos" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _supplyCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Models.SupplyCategory>()), Times.Never);
        }

        [Fact]
        public async Task Handle_CategoryExists_UpdatesSuccessfully()
        {
            var existing = new Domain.Models.SupplyCategory { Id = 1, Name = "Medicamentos" };

            _supplyCategoryRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);
            _supplyCategoryRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Models.SupplyCategory>()))
                .ReturnsAsync((Domain.Models.SupplyCategory c) => c);

            var command = new UpdateCommand { Id = 1, Name = "Medicamentos veterinarios" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _supplyCategoryRepository.Verify(r => r.UpdateAsync(It.Is<Domain.Models.SupplyCategory>(c =>
                c.Name == "Medicamentos veterinarios")), Times.Once);
        }
    }
}
