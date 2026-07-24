using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.States.Commands.DeleteStates
{
    public class DeleteStatesCommandHandler
        : IRequestHandler<DeleteStatesCommand, int?>
    {
        private readonly IStateRepository _stateRepository;

        public DeleteStatesCommandHandler(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<int?> Handle(DeleteStatesCommand command, CancellationToken cancellationToken)
        {            
            var state = await _stateRepository.GetById(command.Id);
         
            if (state is null) return null;
                       
            await _stateRepository.DeleteState(command.Id);
            return state.IdState;
        }
    }
}
