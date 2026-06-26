// Handler que cambia el estado activo/inactivo de una mascota
using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Pets.Commands.ChangeStatePet
{
    public class ChangeStatePetCommandHandler
        : IRequestHandler<ChangeStatePetCommand, int?>
    {
        private readonly IPetRepository _petRepository;

        public ChangeStatePetCommandHandler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<int?> Handle(ChangeStatePetCommand request, CancellationToken cancellationToken)
        {
            // Verificamos que la mascota exista antes de cambiar su estado
            var pet = await _petRepository.GetByIdAsync(request.PetId);
            if (pet == null) return null;

            // No hacemos toggle: usamos el valor exacto que llega en el request
            // true → activa, false → inactiva
            await _petRepository.ChangePetState(request.PetId, request.IsActive);
            return request.PetId;
        }
    }
}
