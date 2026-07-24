using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Pets.Commands.UpdatePet
{
    public class UpdatePetCommandHandler
        : IRequestHandler<UpdatePetCommand, int?>
    {
        private readonly IPetRepository _petRepository;

        public UpdatePetCommandHandler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<int?> Handle(
            UpdatePetCommand request, CancellationToken cancellationToken)
        {            
            var pet = await _petRepository.GetByIdAsync(request.PetId);
            
            if (pet == null) return null;
            
            pet.PetName  = request.PetName;
            pet.SpecieId = request.SpecieId;
            pet.OwnerId  = request.OwnerId;
            
            await _petRepository.UpdatePet(request.PetId, pet);
            return request.PetId;
        }
    }
}