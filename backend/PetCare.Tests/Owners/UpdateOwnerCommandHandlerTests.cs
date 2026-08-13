using Moq;
using PetCare.Application.Owners.Commands.UpdateOwner;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Owners
{
    public class UpdateOwnerCommandHandlerTests
    {
        private readonly Mock<IOwnerRepository> _ownerRepository;
        private readonly UpdateOwnerCommandHandler _handler;


        public UpdateOwnerCommandHandlerTests()
        {
            _ownerRepository = new Mock<IOwnerRepository>();
            _handler = new UpdateOwnerCommandHandler(_ownerRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidOwner_ReturnsUpdatedOwnerId()
        {
            _ownerRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Owner { Id = 7, OwnerName = "Viejo Nombre", Email = "viejo@petcare.com" });

            _ownerRepository
                .Setup(r => r.UpdateOwner(It.IsAny<Owner>()))
                .ReturnsAsync(new Owner { Id = 7, OwnerName = "Jose", Email = "jose@petcare.com" });

            var command = new UpdateOwnerCommand
            {
                OwnerId = 7,
                OwnerName = "Jose",
                Email = "jose@petcare.com",
                PhoneNumber = "123456789",
                Gender = "Masculino",
                IdCard = "123456789"


            };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(7, result);
        }
        [Fact]       
        public async Task Handle_OwnerDoesNotExist_ReturnsNull()
        {
            _ownerRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync((Owner?)null);

            var command = new UpdateOwnerCommand { OwnerId = 7, OwnerName = "Jose", Email = "jose@petcare.com" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
            
            _ownerRepository.Verify(r => r.UpdateOwner(It.IsAny<Owner>()), Times.Never);
        }

        [Fact]

        public async Task Handle_RepositoryFailsToUpdate_ReturnsNull()
        {
            _ownerRepository
                .Setup(r => r.UpdateOwner(It.IsAny<Owner>()))
                .ReturnsAsync((Owner?)null);
            var command = new UpdateOwnerCommand
            {
                OwnerId = 7,
                OwnerName = "Jose",
                Email = "jose@petcare.com",
                PhoneNumber = "123456789",
                Gender = "Masculino",
                IdCard = "123456789"
            };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ValidOwner_MapsFieldsCorrectlyBeforeSaving()
        {            
            _ownerRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Owner { Id = 7, OwnerName = "Viejo Nombre", Email = "viejo@petcare.com" });

            Owner? capturedOwner = null;
           
            _ownerRepository
                .Setup(r => r.UpdateOwner(It.IsAny<Owner>()))
                .Callback<Owner>(o => capturedOwner = o)
                .ReturnsAsync(new Owner { Id = 7, OwnerName = "Jose", Email = "jose@petcare.com" });

            var command = new UpdateOwnerCommand
            {
                OwnerId = 7,
                OwnerName = "Jose",
                Email = "jose@petcare.com",
                PhoneNumber = "123456789",
                Gender = "Masculino",
                IdCard = "123456789"
            };

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedOwner);
            Assert.Equal(7, capturedOwner!.Id);
            Assert.Equal("Jose", capturedOwner.OwnerName);
            Assert.Equal("jose@petcare.com", capturedOwner.Email);
            Assert.Equal("123456789", capturedOwner.PhoneNumber);
            Assert.Equal("Masculino", capturedOwner.Gender);
            Assert.Equal("123456789", capturedOwner.IdCard);

            _ownerRepository.Verify(r => r.UpdateOwner(It.IsAny<Owner>()), Times.Once);
        }
    }
}
