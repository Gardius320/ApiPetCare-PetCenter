using Moq;
using PetCare.Application.Owners.Commands.CreateOwner;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Owners
{
    public class CreateOwnerCommandHandlerTests
    {
        private readonly Mock<IOwnerRepository> _ownerRepository;
        private readonly CreateOwnerCommandHandler _handler;

        public CreateOwnerCommandHandlerTests()
        {
            _ownerRepository = new Mock<IOwnerRepository>();
            _handler = new CreateOwnerCommandHandler(_ownerRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidOwner_ReturnsCreatedOwnerId()
        {
            _ownerRepository
                .Setup(r => r.CreateOwner(It.IsAny<Owner>()))
                .ReturnsAsync(new Owner { Id = 7, OwnerName = "Jose", Email = "jose@petcare.com" });

            var command = new CreateOwnerCommand
            {
                OwnerName = "Jose",
                Email = "jose@petcare.com",
                PhoneNumber = "0999999999",
                IdCard = "1234567890",
                Gender = "Masculino"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(7, result);
        }

        [Fact]
        public async Task Handle_RepositoryFailsToCreate_ReturnsNull()
        {
            _ownerRepository
                .Setup(r => r.CreateOwner(It.IsAny<Owner>()))
                .ReturnsAsync((Owner?)null);

            var command = new CreateOwnerCommand { OwnerName = "Jose", Email = "jose@petcare.com" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ValidOwner_MapsFieldsCorrectlyBeforeSaving()
        {
            Owner? capturedOwner = null;

            _ownerRepository
                .Setup(r => r.CreateOwner(It.IsAny<Owner>()))
                .Callback<Owner>(o => capturedOwner = o)
                .ReturnsAsync(new Owner { Id = 1, OwnerName = "Jose", Email = "jose@petcare.com" });

            var command = new CreateOwnerCommand
            {
                OwnerName = "Jose",
                Email = "jose@petcare.com",
                PhoneNumber = "0999999999",
                IdCard = "1234567890",
                Gender = "Masculino"
            };

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedOwner);
            Assert.Equal("Jose", capturedOwner!.OwnerName);
            Assert.Equal("jose@petcare.com", capturedOwner.Email);
            Assert.Equal("0999999999", capturedOwner.PhoneNumber);
            Assert.Equal("1234567890", capturedOwner.IdCard);
            Assert.Equal("Masculino", capturedOwner.Gender);

            _ownerRepository.Verify(r => r.CreateOwner(It.IsAny<Owner>()), Times.Once);
        }
    }
}
