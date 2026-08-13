using Moq;
using PetCare.Application.Species.Commands.DeleteSpecies;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;


public class DeleteCommandHandlerTests
{
    private readonly Mock<ISpeciesRepository> _speciesRepository;
    private readonly DeleteSpeciesCommandHandler _handler;

    public DeleteCommandHandlerTests()
    {
        _speciesRepository = new Mock<ISpeciesRepository>();
        _handler = new DeleteSpeciesCommandHandler(_speciesRepository.Object);
    }

    [Fact]

    public async Task Handle_ValidId_DelegatesDEleteToRepositoryAndReturnItsMessage()
    {

        var speciesId = 7;
        var expectedMessage = "Especie eliminada correctamente";
        _speciesRepository
            .Setup(r => r.DeleteSpecies(7))
            .ReturnsAsync(expectedMessage);

        var command = new DeleteSpeciesCommand { Id = speciesId };

        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Equal(expectedMessage, result);

        _speciesRepository.Verify(r => r.DeleteSpecies(speciesId), Times.Once);

    }
}
