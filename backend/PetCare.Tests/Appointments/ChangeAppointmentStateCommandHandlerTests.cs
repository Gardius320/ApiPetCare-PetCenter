using Moq;
using PetCare.Application.Appointments.Commands.ChangeAppointmentState;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Appointments
{
    // Este handler es un simple pasamanos hacia el repositorio, sin ninguna
    // rama de lógica propia. Es el caso ideal para [Theory]/[InlineData]:
    // el mismo cuerpo de test, corrido con true y con false.
    public class ChangeAppointmentStateCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepository;
        private readonly ChangeAppointmentStateCommandHandler _handler;

        public ChangeAppointmentStateCommandHandlerTests()
        {
            _appointmentRepository = new Mock<IAppointmentRepository>();
            _handler = new ChangeAppointmentStateCommandHandler(_appointmentRepository.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_DelegatesToRepositoryAndReturnsItsResult(bool repositoryResult)
        {
            _appointmentRepository
                .Setup(r => r.ChangeAppointmentState(1, 3))
                .ReturnsAsync(repositoryResult);

            var command = new ChangeAppointmentStateCommand { Id = 1, StateId = 3 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(repositoryResult, result);
            _appointmentRepository.Verify(r => r.ChangeAppointmentState(1, 3), Times.Once);
        }
    }
}
