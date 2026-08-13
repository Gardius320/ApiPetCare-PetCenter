using Moq;
using PetCare.Application.Owners.Commands.DeleteOwner;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Owners
{
    public class DeleteOwnerCommandHandlerTests
    {
        private readonly Mock<IOwnerRepository> _ownerRepository;
        private readonly DeleteOwnerCommandHandler _handler;

        public DeleteOwnerCommandHandlerTests()
        {
            _ownerRepository = new Mock<IOwnerRepository>();
            _handler = new DeleteOwnerCommandHandler(_ownerRepository.Object);
        }

        [Fact]
        public async Task Handle_OwnerDoesNotExist_ReturnsFailure()
        {
            _ownerRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync((Owner?)null);

            var command = new DeleteOwnerCommand { Id = 7 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Propietario no encontrado", result.Message);

           
            _ownerRepository.Verify(r => r.GetDependencyCountsAsync(It.IsAny<int>()), Times.Never);
            _ownerRepository.Verify(r => r.DeleteOwner(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_OwnerHasPetsOrAppointments_ReturnsFailure()
        {
            _ownerRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Owner { Id = 7, OwnerName = "Jose", Email = "jose@petcare.com" });

            _ownerRepository
                .Setup(r => r.GetDependencyCountsAsync(7))
                .ReturnsAsync((petsCount: 2, appointmentsCount: 0));

            var command = new DeleteOwnerCommand { Id = 7 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains("2 mascota", result.Message);

            _ownerRepository.Verify(r => r.DeleteOwner(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_OwnerWithNoDependencies_DeletesSuccessfully()
        {
            _ownerRepository
                .Setup(r => r.GetByIdAsync(7))
                .ReturnsAsync(new Owner { Id = 7, OwnerName = "Jose", Email = "jose@petcare.com" });

            _ownerRepository
                .Setup(r => r.GetDependencyCountsAsync(7))
                .ReturnsAsync((petsCount: 0, appointmentsCount: 0));

            _ownerRepository
                .Setup(r => r.DeleteOwner(7))
                .ReturnsAsync("Propietario eliminado correctamente");

            var command = new DeleteOwnerCommand { Id = 7 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("Propietario eliminado correctamente", result.Message);

            _ownerRepository.Verify(r => r.DeleteOwner(7), Times.Once);
        }
    }
}
