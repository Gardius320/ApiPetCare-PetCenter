using Moq;
using PetCare.Application.Supplies.Commands.CreateSupply;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Supplies
{
    public class CreateSupplyCommandHandlerTests
    {
        private readonly Mock<ISupplyRepository> _supplyRepository;
        private readonly CreateSupplyCommandHandler _handler;

        public CreateSupplyCommandHandlerTests()
        {
            _supplyRepository = new Mock<ISupplyRepository>();
            _handler = new CreateSupplyCommandHandler(_supplyRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidSupply_ReturnsSuccess()
        {
            _supplyRepository
                .Setup(r => r.CreateSupply(It.IsAny<Domain.Models.Supply>()))
                .ReturnsAsync((Domain.Models.Supply s) => s);

            var command = new CreateSupplyCommand
            {
                Name = "Jeringas 5ml",
                Unit = "unidad",
                CurrentStock = 100,
                MinimumStock = 20,
                SupplyCategoryId = 1,
                SupplyType = SupplyType.Clinical
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_RepositoryFailsToCreate_ReturnsFailure()
        {
            // A diferencia de MedicalRecord/Service/SupplyCategories, este handler
            // SÍ revisa si el repositorio devolvió null antes de responder.
            _supplyRepository
                .Setup(r => r.CreateSupply(It.IsAny<Domain.Models.Supply>()))
                .ReturnsAsync((Domain.Models.Supply?)null);

            var command = new CreateSupplyCommand
            {
                Name = "Jeringas 5ml",
                Unit = "unidad",
                CurrentStock = 100,
                MinimumStock = 20,
                SupplyCategoryId = 1,
                SupplyType = SupplyType.Clinical
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
