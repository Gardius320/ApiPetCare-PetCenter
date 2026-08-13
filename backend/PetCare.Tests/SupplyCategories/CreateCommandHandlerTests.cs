using Moq;
using PetCare.Application.SupplyCategories.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.SupplyCategories
{
    public class CreateCommandHandlerTests
    {
        private readonly Mock<ISupplyCategoryRepository> _supplyCategoryRepository;
        private readonly CreateCommandHandler _handler;

        public CreateCommandHandlerTests()
        {
            _supplyCategoryRepository = new Mock<ISupplyCategoryRepository>();
            _handler = new CreateCommandHandler(_supplyCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidCategory_ReturnsGeneratedId()
        {
            _supplyCategoryRepository
                .Setup(r => r.Create(It.IsAny<Domain.Models.SupplyCategory>()))
                .Callback<Domain.Models.SupplyCategory>(c => c.Id = 4)
                .ReturnsAsync((Domain.Models.SupplyCategory c) => c);

            var command = new CreateCommand { Name = "Medicamentos", Description = "Categoría de medicamentos" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(4, result);
        }

        [Fact]
        public async Task Handle_ValidCategory_MapsFieldsCorrectlyBeforeSaving()
        {
            _supplyCategoryRepository
                .Setup(r => r.Create(It.IsAny<Domain.Models.SupplyCategory>()))
                .ReturnsAsync((Domain.Models.SupplyCategory c) => c);

            var command = new CreateCommand { Name = "Alimentos", Description = "Alimentos y snacks" };

            await _handler.Handle(command, CancellationToken.None);

            _supplyCategoryRepository.Verify(r => r.Create(It.Is<Domain.Models.SupplyCategory>(c =>
                c.Name == "Alimentos" && c.Description == "Alimentos y snacks")), Times.Once);
        }
    }
}
