using Moq;
using PetCare.Application.Pets.Commands.UpdatePet;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Pets
{
    public class UpdatePetCommandHandlerTests
    {

        private readonly Mock<IPetRepository> _petRepository;
        private readonly UpdatePetCommandHandler _handler;
        public UpdatePetCommandHandlerTests()
        {
            _petRepository = new Mock<IPetRepository>();
            _handler = new UpdatePetCommandHandler(_petRepository.Object);
        }

        [Fact]

        public async Task Handle_ValidPet_ReturnsUpdatedPetId()
        {
            _petRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Domain.Models.Pet { Id = 7, PetName = "Old Name" });
            _petRepository
                .Setup(r => r.UpdatePet(It.IsAny<int>(), It.IsAny<Domain.Models.Pet>()))
                .ReturnsAsync(new Domain.Models.Pet { Id = 7, PetName = "New Name" });
            var command = new UpdatePetCommand
            {
                PetId = 7,
                PetName = "New Name",
                SpecieId = 1,
                OwnerId = 1
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.Equal(7, result);
        }

        [Fact]

        public async Task Handle_PetDoesNotExist_ReturnsNull()
        {
            _petRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync((Domain.Models.Pet?)null);
            var command = new UpdatePetCommand { PetId = 7, PetName = "New Name", SpecieId = 1, OwnerId = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.Null(result);
            _petRepository.Verify(r => r.UpdatePet(It.IsAny<int>(), It.IsAny<Domain.Models.Pet>()), Times.Never);

        }

        [Fact]
        public async Task Handle_PetExists_MapsFieldsCorrectlyBeforeSaving()
        {
            var existingPet = new Domain.Models.Pet
            {
                Id = 7,
                PetName = "Old Name",
                SpecieId = 1,
                OwnerId = 1
            };
            _petRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(existingPet);
            _petRepository
                .Setup(r => r.UpdatePet(It.IsAny<int>(), It.IsAny<Domain.Models.Pet>()))
                .ReturnsAsync((int id, Domain.Models.Pet pet) => pet);
            var command = new UpdatePetCommand
            {
                PetId = 7,
                PetName = "New Name",
                SpecieId = 2,
                OwnerId = 2
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.Equal(7, result);
            _petRepository.Verify(r => r.UpdatePet(7, It.Is<Domain.Models.Pet>(p =>
                p.PetName == "New Name" &&
                p.SpecieId == 2 &&
                p.OwnerId == 2)), Times.Once);
             }
        }
}
