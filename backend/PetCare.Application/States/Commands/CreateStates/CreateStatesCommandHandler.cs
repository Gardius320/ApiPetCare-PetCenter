using MediatR;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.States.Commands.CreateStates
{
    public class CreateStatesCommandHandler
        : IRequestHandler<CreateStatesCommand, int?>
    {
        private readonly IStateRepository _stateRepository;

        public CreateStatesCommandHandler(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<int?> Handle(CreateStatesCommand request, CancellationToken cancellationToken)
        {            
            var state = new State
            {
                StateName   = request.StateName,
                Description = request.Description
            };
           
            var created = await _stateRepository.CreateState(state);
                        
            if (created == null) return null;

            return created.IdState;
        }
    }
}
