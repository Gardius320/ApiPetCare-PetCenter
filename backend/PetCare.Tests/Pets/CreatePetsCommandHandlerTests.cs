using Moq;
using PetCare.Application.Pets.Commands.CreatePet;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Pets
{
    public class CreatePetCommandHandlerTests
    {

        private readonly Mock<IPetRepository> _petRepository;
        private readonly CreatePetCommandHandler _handler;
        public CreatePetCommandHandlerTests()
        {
            _petRepository = new Mock<IPetRepository>();
            _handler = new CreatePetCommandHandler(_petRepository.Object);
        }

        [Fact]

        public async Task Handle_ValidPet_ReturnCreatePetId()
        {
            _petRepository
                .Setup(r => r.CreatePetAsync(It.IsAny<Domain.Models.Pet>()))
                .ReturnsAsync(new Domain.Models.Pet { Id = 1 });

            var command = new CreatePetCommand
            {
                PetName = "Buddy",
                SpecieId = 1,
                OwnerId = 1
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(1, result);
        }

        [Fact]

        public async Task Handle_ValidPet_MapsFieldsCorrectlyBeforeSaving()
        {
            Domain.Models.Pet? capturedPet = null;
            _petRepository
                .Setup(r => r.CreatePetAsync(It.IsAny<Domain.Models.Pet>()))
                .Callback<Domain.Models.Pet>(pet => capturedPet = pet)
                .ReturnsAsync(new Domain.Models.Pet { Id = 1 });
            var command = new CreatePetCommand
            {
                PetName = "Buddy",
                SpecieId = 1,
                OwnerId = 1
            };
            await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(capturedPet);
            Assert.Equal("Buddy", capturedPet?.PetName);
            Assert.Equal(1, capturedPet?.SpecieId);
            Assert.Equal(1, capturedPet?.OwnerId);

            _petRepository.Verify(r => r.CreatePetAsync(It.IsAny<Domain.Models.Pet>()), Times.Once);
        }
    }
}
