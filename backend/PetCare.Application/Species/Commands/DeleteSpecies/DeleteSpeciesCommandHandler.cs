using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Species.Commands.DeleteSpecies
{
    public class DeleteSpeciesCommandHandler : IRequestHandler<DeleteSpeciesCommand, string>
    {
        private readonly ISpeciesRepository _speciesRepository;

        public DeleteSpeciesCommandHandler(ISpeciesRepository speciesRepository)
        {
            _speciesRepository = speciesRepository;
        }

        public async Task<string> Handle(DeleteSpeciesCommand command, CancellationToken cancellationToken)
        {
            
            return await _speciesRepository.DeleteSpecies(command.Id);
        }
    }
}