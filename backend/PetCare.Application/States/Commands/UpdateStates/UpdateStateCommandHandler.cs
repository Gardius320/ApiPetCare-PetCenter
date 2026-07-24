using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.States.Commands.UpdateState
{
    public class UpdateStateCommandHandler
        : IRequestHandler<UpdateStateCommand, int?>
    {
        private readonly IStateRepository _stateRepository;

        public UpdateStateCommandHandler(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<int?> Handle(UpdateStateCommand request, CancellationToken cancellationToken)
        {            
            var existingState = await _stateRepository.GetById(request.Id);

            if (existingState == null)
            {
                return null;
            }
            
            existingState.StateName   = request.StateName;
            existingState.Description = request.StateDescription;
           
            var updated = await _stateRepository.UpdateAsync(existingState);
            return updated.IdState;
        }
    }
}
