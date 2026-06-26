// Handler que busca el estado, actualiza sus datos y los guarda
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
            // Buscamos el estado por Id
            var existingState = await _stateRepository.GetById(request.Id);

            // Si no existe, devolvemos null para que el controlador responda 404
            if (existingState == null)
            {
                return null;
            }

            // Actualizamos los campos con los nuevos valores
            existingState.StateName   = request.StateName;
            existingState.Description = request.StateDescription;

            // Guardamos y devolvemos el Id del estado actualizado
            var updated = await _stateRepository.UpdateAsync(existingState);
            return updated.IdState;
        }
    }
}
