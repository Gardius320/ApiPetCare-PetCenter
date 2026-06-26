// Handler que verifica que el estado exista y luego lo elimina
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
            // Buscamos el estado para confirmar que existe
            // El id viene en command.Id (no en StateId, ese siempre llegaba en 0)
            var state = await _stateRepository.GetById(command.Id);

            // Si no existe, devolvemos null para que el controlador responda 404
            if (state is null) return null;

            // Eliminamos el estado
            await _stateRepository.DeleteState(command.Id);
            return state.IdState;
        }
    }
}
