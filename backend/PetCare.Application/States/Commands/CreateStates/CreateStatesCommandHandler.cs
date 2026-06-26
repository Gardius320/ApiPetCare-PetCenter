// Handler que crea un nuevo estado y lo guarda en la base de datos
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
            // Armamos el objeto State con los datos recibidos
            var estado = new State
            {
                StateName   = request.StateName,
                Description = request.Description
            };

            // Guardamos el estado
            var creado = await _stateRepository.CreateState(estado);

            // Si algo falló, devolvemos null
            if (creado == null) return null;

            return creado.IdState;
        }
    }
}
