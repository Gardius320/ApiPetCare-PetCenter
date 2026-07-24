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
            
            var pet = await _petRepository.GetByIdAsync(request.PetId);
            if (pet == null) return null;
            
            await _petRepository.ChangePetState(request.PetId, request.IsActive);
            return request.PetId;
        }
    }
}
