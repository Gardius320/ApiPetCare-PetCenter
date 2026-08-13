using Moq;
using PetCare.Application.Appointments.Commands.CreateAppointment;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Appointments
{
    public class CreateAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
        private readonly Mock<IOwnerRepository> _ownerRepository = new();
        private readonly Mock<IPetRepository> _petRepository = new();
        private readonly Mock<IEmailService> _emailService = new();
        private readonly CreateAppointmentCommandHandler _handler;

        public CreateAppointmentCommandHandlerTests()
        {
            _handler = new CreateAppointmentCommandHandler(
                _appointmentRepository.Object,
                _ownerRepository.Object,
                _petRepository.Object,
                _emailService.Object);
        }

        [Fact]
        public async Task Handle_RepositoryFailsToCreate_ReturnsNull()
        {
            _appointmentRepository
                .Setup(r => r.CreateAppointment(1, It.IsAny<DateTime>(), It.IsAny<string>(), null, null))
                .ReturnsAsync((Appointment?)null);

            var command = new CreateAppointmentCommand { OwnerId = 1, AppointmentDate = DateTime.Today };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
            _emailService.Verify(e => e.SendAppointmentConfirmationAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ValidAppointmentWithoutPet_ReturnsIdAndDoesNotSendEmail()
        {
            _appointmentRepository
                .Setup(r => r.CreateAppointment(1, It.IsAny<DateTime>(), It.IsAny<string>(), null, null))
                .ReturnsAsync(new Appointment { Id = 10, OwnerId = 1 });

            var command = new CreateAppointmentCommand { OwnerId = 1, AppointmentDate = DateTime.Today };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(10, result);
            _emailService.Verify(e => e.SendAppointmentConfirmationAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ValidAppointmentWithPet_SendsConfirmationEmail()
        {
            var appointmentDate = DateTime.Today.AddDays(2);
            var owner = new Owner { Id = 1, OwnerName = "Jose", Email = "jose@petcare.com" };
            var pet = new Pet { Id = 5, PetName = "Firulais" };

            _appointmentRepository
                .Setup(r => r.CreateAppointment(1, appointmentDate, It.IsAny<string>(), 5, null))
                .ReturnsAsync(new Appointment { Id = 10, OwnerId = 1, PetId = 5 });
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(owner);
            _petRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(pet);

            var command = new CreateAppointmentCommand { OwnerId = 1, PetId = 5, AppointmentDate = appointmentDate };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(10, result);
            _emailService.Verify(e => e.SendAppointmentConfirmationAsync(
                "jose@petcare.com", "Jose", "Firulais", appointmentDate, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_PetProvidedButNotFound_DoesNotSendEmail()
        {
            _appointmentRepository
                .Setup(r => r.CreateAppointment(1, It.IsAny<DateTime>(), It.IsAny<string>(), 5, null))
                .ReturnsAsync(new Appointment { Id = 10, OwnerId = 1, PetId = 5 });
            _ownerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Owner { Id = 1, OwnerName = "Jose", Email = "jose@petcare.com" });
            _petRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Pet?)null);

            var command = new CreateAppointmentCommand { OwnerId = 1, PetId = 5, AppointmentDate = DateTime.Today };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(10, result);
            _emailService.Verify(e => e.SendAppointmentConfirmationAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
