using Moq;
using PetCare.Application.SupplyCategories.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.SupplyCategories
{
    public class DeleteCommandHandlerTests
    {
        private readonly Mock<ISupplyCategoryRepository> _supplyCategoryRepository;
        private readonly DeleteCommandHandler _handler;

        public DeleteCommandHandlerTests()
        {
            _supplyCategoryRepository = new Mock<ISupplyCategoryRepository>();
            _handler = new DeleteCommandHandler(_supplyCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_CategoryDoesNotExist_ReturnsFailure()
        {
            _supplyCategoryRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Domain.Models.SupplyCategory?)null);

            var command = new DeleteCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _supplyCategoryRepository.Verify(r => r.DeleteAsync(It.IsAny<Domain.Models.SupplyCategory>()), Times.Never);
        }

        [Fact]
        public async Task Handle_CategoryExists_DeletesSuccessfully()
        {
            var existing = new Domain.Models.SupplyCategory { Id = 1, Name = "Medicamentos" };

            _supplyCategoryRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);
            _supplyCategoryRepository
                .Setup(r => r.DeleteAsync(existing))
                .ReturnsAsync(existing);

            var command = new DeleteCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            // Este Success solo recibe un argumento (el mensaje fijo), por lo que
            // va como Data; el Message queda con el valor por defecto de ApiResponse.
            Assert.Equal("Categoría eliminada correctamente", result.Data);
            _supplyCategoryRepository.Verify(r => r.DeleteAsync(existing), Times.Once);
        }
    }
}
