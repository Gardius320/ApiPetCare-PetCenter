using Moq;
using PetCare.Application.Appointments.Commands.DeleteAppointment;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Appointments
{
    public class DeleteAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepository;
        private readonly DeleteAppointmentCommandHandler _handler;

        public DeleteAppointmentCommandHandlerTests()
        {
            _appointmentRepository = new Mock<IAppointmentRepository>();
            _handler = new DeleteAppointmentCommandHandler(_appointmentRepository.Object);
        }

        [Fact]
        public async Task Handle_AppointmentDoesNotExist_ReturnsNull()
        {
            _appointmentRepository.Setup(r => r.CancelAppointment(999)).ReturnsAsync(false);

            var command = new DeleteAppointmentCommand { Id = 999 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_AppointmentExists_CancelsItAndReturnsId()
        {
            _appointmentRepository.Setup(r => r.CancelAppointment(42)).ReturnsAsync(true);

            var command = new DeleteAppointmentCommand { Id = 42 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(42, result);
            _appointmentRepository.Verify(r => r.CancelAppointment(42), Times.Once);
        }
    }
}
