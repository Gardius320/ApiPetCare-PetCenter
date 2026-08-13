using Moq;
using PetCare.Application.States.Commands.DeleteStates;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.States
{
    public class DeleteCommandHandlerTests
    {
        private readonly Mock<IStateRepository> _stateRepository;
        private readonly DeleteStatesCommandHandler _handler;

        public DeleteCommandHandlerTests()
        {
            _stateRepository = new Mock<IStateRepository>();
            _handler = new DeleteStatesCommandHandler(_stateRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidId_DeletesAndReturnsStateId()
        {
            var stateId = 7;
            var existingState = new Domain.Models.State { IdState = stateId, StateName = "Activo" };

            _stateRepository
                .Setup(r => r.GetById(stateId))
                .ReturnsAsync(existingState);
            _stateRepository
                .Setup(r => r.DeleteState(stateId))
                .ReturnsAsync("Estado eliminado correctamente");

            var command = new DeleteStatesCommand { Id = stateId };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(stateId, result);
            _stateRepository.Verify(r => r.DeleteState(stateId), Times.Once);
        }

        [Fact]
        public async Task Handle_StateDoesNotExist_ReturnsNull()
        {
            _stateRepository
                .Setup(r => r.GetById(7))
                .ReturnsAsync((Domain.Models.State?)null);

            var command = new DeleteStatesCommand { Id = 7 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
            _stateRepository.Verify(r => r.DeleteState(7), Times.Never);
        }
    }
}
