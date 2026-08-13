using Moq;
using PetCare.Application.States.Commands.UpdateState;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.States
{
    public class UpdateCommandHandlerTests
    {
        private readonly Mock<IStateRepository> _stateRepository;
        private readonly UpdateStateCommandHandler _handler;

        public UpdateCommandHandlerTests()
        {
            _stateRepository = new Mock<IStateRepository>();
            _handler = new UpdateStateCommandHandler(_stateRepository.Object);
        }

        [Fact]
        public async Task Handle_StateDoesNotExist_ReturnsNull()
        {
            _stateRepository
                .Setup(r => r.GetById(1))
                .ReturnsAsync((Domain.Models.State?)null);

            var command = new UpdateStateCommand { Id = 1, StateName = "Activo" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
            _stateRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Models.State>()), Times.Never);
        }

        [Fact]
        public async Task Handle_StateExists_UpdatesSuccessfully()
        {
            var existingState = new Domain.Models.State
            {
                IdState = 1,
                StateName = "Inactivo",
                Description = "Descripción vieja"
            };

            _stateRepository
                .Setup(r => r.GetById(1))
                .ReturnsAsync(existingState);
            _stateRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Models.State>()))
                .ReturnsAsync((Domain.Models.State state) => state);

            var command = new UpdateStateCommand
            {
                Id = 1,
                StateName = "Activo",
                StateDescription = "Descripción nueva"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(1, result);
            _stateRepository.Verify(r => r.UpdateAsync(It.Is<Domain.Models.State>(s =>
                s.StateName == "Activo" &&
                s.Description == "Descripción nueva")), Times.Once);
        }
    }
}
