using Moq;
using PetCare.Application.Services.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Services
{
    public class CreateCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepository;
        private readonly CreateCommandHandler _handler;

        public CreateCommandHandlerTests()
        {
            _serviceRepository = new Mock<IServiceRepository>();
            _handler = new CreateCommandHandler(_serviceRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidService_ReturnsGeneratedId()
        {
            // El handler retorna service.Id (el objeto local), no lo que
            // devuelve el repositorio. Simulamos que el repositorio asigna
            // el Id mutando el mismo objeto, como hace EF Core al guardar.
            _serviceRepository
                .Setup(r => r.CreateService(It.IsAny<Domain.Models.Service>()))
                .Callback<Domain.Models.Service>(s => s.Id = 8)
                .ReturnsAsync((Domain.Models.Service s) => s);

            var command = new CreateCommand
            {
                Name = "Baño y corte",
                Description = "Baño completo con corte de pelo",
                Price = 45000
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(8, result);
        }

        [Fact]
        public async Task Handle_ValidService_MapsFieldsCorrectlyBeforeSaving()
        {
            _serviceRepository
                .Setup(r => r.CreateService(It.IsAny<Domain.Models.Service>()))
                .ReturnsAsync((Domain.Models.Service s) => s);

            var command = new CreateCommand
            {
                Name = "Vacunación",
                Description = "Aplicación de vacuna anual",
                Price = 30000
            };

            await _handler.Handle(command, CancellationToken.None);

            _serviceRepository.Verify(r => r.CreateService(It.Is<Domain.Models.Service>(s =>
                s.Name == "Vacunación" &&
                s.Description == "Aplicación de vacuna anual" &&
                s.Price == 30000)), Times.Once);
        }
    }
}
