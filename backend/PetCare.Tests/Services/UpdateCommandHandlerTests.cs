using Moq;
using PetCare.Application.Services.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Services
{
    public class UpdateCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepository;
        private readonly UpdateCommandHandler _handler;

        public UpdateCommandHandlerTests()
        {
            _serviceRepository = new Mock<IServiceRepository>();
            _handler = new UpdateCommandHandler(_serviceRepository.Object);
        }

        [Fact]
        public async Task Handle_ServiceDoesNotExist_ReturnsFailure()
        {
            _serviceRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Domain.Models.Service?)null);

            var command = new UpdateCommand { Id = 1, Name = "Baño", Price = 20000 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _serviceRepository.Verify(r => r.UpdateService(It.IsAny<Domain.Models.Service>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ServiceExists_UpdatesSuccessfully()
        {
            var existing = new Domain.Models.Service { Id = 1, Name = "Baño", Price = 20000 };

            _serviceRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);
            _serviceRepository
                .Setup(r => r.UpdateService(It.IsAny<Domain.Models.Service>()))
                .ReturnsAsync((Domain.Models.Service s) => s);

            var command = new UpdateCommand { Id = 1, Name = "Baño premium", Price = 35000 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _serviceRepository.Verify(r => r.UpdateService(It.Is<Domain.Models.Service>(s =>
                s.Name == "Baño premium" && s.Price == 35000)), Times.Once);
        }
    }
}
