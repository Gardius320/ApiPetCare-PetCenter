using Moq;
using PetCare.Application.Appointments.Commands.UpdateAppointment;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Appointments
{
    public class UpdateAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepository;
        private readonly UpdateAppointmentCommandHandler _handler;

        public UpdateAppointmentCommandHandlerTests()
        {
            _appointmentRepository = new Mock<IAppointmentRepository>();
            _handler = new UpdateAppointmentCommandHandler(_appointmentRepository.Object);
        }

        [Fact]
        public async Task Handle_RepositoryFailsToUpdate_ReturnsNull()
        {
            _appointmentRepository
                .Setup(r => r.UpdateAppointment(1, It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var command = new UpdateAppointmentCommand { Id = 1, AppointmentDate = DateTime.Today };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ValidUpdate_ReturnsAppointmentId()
        {
            _appointmentRepository
                .Setup(r => r.UpdateAppointment(1, It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var command = new UpdateAppointmentCommand { Id = 1, AppointmentDate = DateTime.Today };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Handle_NullObservation_IsSentAsEmptyString()
        {
            var appointmentDate = DateTime.Today.AddDays(1);

            _appointmentRepository
                .Setup(r => r.UpdateAppointment(1, appointmentDate, string.Empty))
                .ReturnsAsync(true);

            var command = new UpdateAppointmentCommand { Id = 1, AppointmentDate = appointmentDate, Observation = null };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(1, result);
            _appointmentRepository.Verify(r => r.UpdateAppointment(1, appointmentDate, string.Empty), Times.Once);
        }
    }
}
