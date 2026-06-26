using MediatR;

namespace PetCare.Application.Species.Commands.DeleteSpecies
{
    public class DeleteSpeciesCommand : IRequest<string>
    {
        
        public int Id { get; set; }
    }
}
