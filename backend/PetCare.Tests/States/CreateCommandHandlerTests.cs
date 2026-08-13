using Moq;
using PetCare.Application.States.Commands.CreateStates;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.States
{
    public class CreateCommandHandlerTests
    {
        private readonly Mock<IStateRepository> _stateRepository;
        private readonly CreateStatesCommandHandler _handler;

        public CreateCommandHandlerTests()
        {
            _stateRepository = new Mock<IStateRepository>();
            _handler = new CreateStatesCommandHandler(_stateRepository.Object);
        }

        [Fact]

        public async Task Handle_ValidStates_ReturnGenerateId() 
        {
            _stateRepository
                .Setup(r => r.CreateState(It.IsAny<PetCare.Domain.Models.State>()))
                .Callback<PetCare.Domain.Models.State>(state => state.IdState = 5)
                .ReturnsAsync((PetCare.Domain.Models.State state) => state);

            var command = new CreateStatesCommand
            {
                StateName = "Test State",
                Description = "Test Description"

            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(5, result);
        }

        [Fact]

        public async Task Handle_RepositoryFailsToCreate_ReturnsNull()
        {
            _stateRepository
                .Setup(r => r.CreateState(It.IsAny<PetCare.Domain.Models.State>()))
                .ReturnsAsync((PetCare.Domain.Models.State?)null);
            var command = new CreateStatesCommand
            {
                StateName = "Test State",
                Description = "Test Description"
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.Null(result);
        }
    }
}
