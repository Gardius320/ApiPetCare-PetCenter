using MediatR;

namespace PetCare.Application.Species.Commands.CreateSpecies
{
    public class CreateSpeciesCommand : IRequest<int?>
    {
       
        public string? SpecieName { get; set; }
    }
}
