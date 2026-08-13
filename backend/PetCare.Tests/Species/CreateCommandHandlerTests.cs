using Moq;
using PetCare.Application.Species.Commands.CreateSpecies;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.Species
{
    public class CreateCommandHandlerTests
    {
        private readonly Mock<ISpeciesRepository> _speciesRepository;
        private readonly CreateSpeciesCommandHandler _handler;

        public CreateCommandHandlerTests()
        {
            _speciesRepository = new Mock<ISpeciesRepository>();
            _handler = new CreateSpeciesCommandHandler(_speciesRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidSpecies_ReturnsGeneratedId()
        {
            _speciesRepository
                .Setup(r => r.CreateSpecies(It.IsAny<PetCare.Domain.Models.Species>()))
                .Callback<PetCare.Domain.Models.Species>(species => species.Id = 5)
                .ReturnsAsync((PetCare.Domain.Models.Species species) => species);

            var command = new CreateSpeciesCommand
            {
                SpecieName = "Canino"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(5, result);
        }

        [Fact]

        public async Task Handle_RepositoryFaiulsToCreate_ReturnsNull()
        {
            _speciesRepository
                .Setup(r => r.CreateSpecies(It.IsAny<PetCare.Domain.Models.Species>()))
                .ReturnsAsync((PetCare.Domain.Models.Species?)null);
            var command = new CreateSpeciesCommand { SpecieName = "Canino" };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.Null(result);
        }
    }
}
