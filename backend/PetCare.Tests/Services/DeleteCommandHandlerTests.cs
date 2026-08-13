using Moq;
using PetCare.Application.Services.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Services
{
    public class DeleteCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepository;
        private readonly DeleteCommandHandler _handler;

        public DeleteCommandHandlerTests()
        {
            _serviceRepository = new Mock<IServiceRepository>();
            _handler = new DeleteCommandHandler(_serviceRepository.Object);
        }
        
        [Fact]
        public async Task Handle_ValidId_CallsRepositoryWithCorrectId()
        {
            _serviceRepository
                .Setup(r => r.DeleteService(5))
                .ReturnsAsync("Servicio eliminado");

            var command = new DeleteCommand { Id = 5 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
           
            Assert.Equal("Servicio eliminado", result.Data);
            _serviceRepository.Verify(r => r.DeleteService(5), Times.Once);
        }
    }
}
