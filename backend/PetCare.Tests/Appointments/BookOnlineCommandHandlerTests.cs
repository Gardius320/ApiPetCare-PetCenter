using Moq;
using PetCare.Application.Appointments.Commands.BookOnline;
using PetCare.Domain.Constants;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Appointments
{
    public class BookOnlineCommandHandlerTests
    {
        private readonly Mock<IOwnerRepository> _ownerRepository = new();
        private readonly Mock<IPetRepository> _petRepository = new();
        private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
        private readonly BookOnlineCommandHandler _handler;

        public BookOnlineCommandHandlerTests()
        {
            _handler = new BookOnlineCommandHandler(
                _ownerRepository.Object,
                _petRepository.Object,
                _appointmentRepository.Object);
        }

        private static BookOnlineCommand BuildCommand() => new()
        {
            OwnerName = "Jose",
            Email = "jose@petcare.com",
            PhoneNumber = "3001234567",
            Gender = "M",
            AppointmentDate = DateTime.Today.AddDays(3),
            PetName = "Firulais",
            SpecieId = 1
        };

        [Fact]
        public async Task Handle_NewOwner_CreatesOwnerPetAndAppointmentAsPendingConfirmation()
        {
            _ownerRepository.Setup(r => r.GetByEmailAsync("jose@petcare.com")).ReturnsAsync((Owner?)null);
            _ownerRepository
                .Setup(r => r.CreateOwner(It.IsAny<Owner>()))
                .ReturnsAsync(new Owner { Id = 1, OwnerName = "Jose", Email = "jose@petcare.com" });
            _petRepository
                .Setup(r => r.CreatePetAsync(It.IsAny<Pet>()))
                .ReturnsAsync(new Pet { Id = 5, PetName = "Firulais", OwnerId = 1 });
            _appointmentRepository
                .Setup(r => r.CreateAppointment(1, It.IsAny<DateTime>(), It.IsAny<string>(), 5, AppointmentStateNames.PendingConfirmation))
                .ReturnsAsync(new Appointment { Id = 10, OwnerId = 1, PetId = 5 });

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.Equal(10, result);
            _ownerRepository.Verify(r => r.CreateOwner(It.IsAny<Owner>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ExistingOwner_ReusesOwnerWithoutCreatingNewOne()
        {
            var existingOwner = new Owner { Id = 1, OwnerName = "Jose", Email = "jose@petcare.com" };
            _ownerRepository.Setup(r => r.GetByEmailAsync("jose@petcare.com")).ReturnsAsync(existingOwner);
            _petRepository
                .Setup(r => r.CreatePetAsync(It.IsAny<Pet>()))
                .ReturnsAsync(new Pet { Id = 5, PetName = "Firulais", OwnerId = 1 });
            _appointmentRepository
                .Setup(r => r.CreateAppointment(1, It.IsAny<DateTime>(), It.IsAny<string>(), 5, AppointmentStateNames.PendingConfirmation))
                .ReturnsAsync(new Appointment { Id = 10, OwnerId = 1, PetId = 5 });

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.Equal(10, result);
            _ownerRepository.Verify(r => r.CreateOwner(It.IsAny<Owner>()), Times.Never);
        }

        [Fact]
        public async Task Handle_OwnerCreationFails_ReturnsNullAndNeverCreatesPet()
        {
            _ownerRepository.Setup(r => r.GetByEmailAsync("jose@petcare.com")).ReturnsAsync((Owner?)null);
            _ownerRepository.Setup(r => r.CreateOwner(It.IsAny<Owner>())).ReturnsAsync((Owner?)null);

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.Null(result);
            _petRepository.Verify(r => r.CreatePetAsync(It.IsAny<Pet>()), Times.Never);
        }

        [Fact]
        public async Task Handle_PetCreationFails_ReturnsNullAndNeverCreatesAppointment()
        {
            _ownerRepository.Setup(r => r.GetByEmailAsync("jose@petcare.com"))
                .ReturnsAsync(new Owner { Id = 1, OwnerName = "Jose", Email = "jose@petcare.com" });
            _petRepository.Setup(r => r.CreatePetAsync(It.IsAny<Pet>())).ReturnsAsync((Pet?)null);

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.Null(result);
            _appointmentRepository.Verify(r => r.CreateAppointment(
                It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string?>()),
                Times.Never);
        }
    }
}
