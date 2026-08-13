using Moq;
using PetCare.Application.Pets.Commands.ChangeStatePet;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Pets
{
    public class ChangeStatePetCommandHandlerTests
    {
        private readonly Mock<IPetRepository> _petRepository;
        private readonly ChangeStatePetCommandHandler _handler;

        public ChangeStatePetCommandHandlerTests()
        {
            _petRepository = new Mock<IPetRepository>();
            _handler = new ChangeStatePetCommandHandler(_petRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidPet_ReturnsPetId()
        {
            // Arrange
            _petRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Domain.Models.Pet { Id = 7, PetName = "Firulais", IsActive = true });
            _petRepository
                .Setup(r => r.ChangePetState(7, false))
                .ReturnsAsync("Estado actualizado correctamente");

            var command = new ChangeStatePetCommand { PetId = 7, IsActive = false };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(7, result);
        }

        [Fact]
        public async Task Handle_PetDoesNotExist_ReturnsNull()
        {
            // Arrange
            _petRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync((Domain.Models.Pet?)null);

            var command = new ChangeStatePetCommand { PetId = 7, IsActive = false };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _petRepository.Verify(r => r.ChangePetState(It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task Handle_PetExists_CallsChangePetStateWithRequestedValues()
        {
            // Arrange
            _petRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Domain.Models.Pet { Id = 7, PetName = "Firulais", IsActive = true });
            _petRepository
                .Setup(r => r.ChangePetState(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync("Estado actualizado correctamente");

            var command = new ChangeStatePetCommand { PetId = 7, IsActive = false };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _petRepository.Verify(r => r.ChangePetState(7, false), Times.Once);
        }
    }
}
