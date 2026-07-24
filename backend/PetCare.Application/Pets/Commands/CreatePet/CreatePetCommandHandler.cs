using MediatR;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Pets.Commands.CreatePet
{
    public class CreatePetCommandHandler
        : IRequestHandler<CreatePetCommand, int?>
    {
        private readonly IPetRepository _petRepository;

        public CreatePetCommandHandler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<int?> Handle(CreatePetCommand request, CancellationToken cancellationToken)
        {            
            var pet = new Pet
            {
                PetName  = request.PetName ?? string.Empty,
                SpecieId = request.SpecieId,
                OwnerId  = request.OwnerId,
                IsActive = true 
            };                        
            var creada = await _petRepository.CreatePetAsync(pet);            
            return creada?.Id;
        }
    }
}
