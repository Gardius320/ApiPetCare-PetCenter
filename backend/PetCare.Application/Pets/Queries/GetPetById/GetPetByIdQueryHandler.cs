using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Pets.Queries.GetPetById
{
    public class GetPetByIdQueryHandler
        : IRequestHandler<GetPetByIdQuery, GetPetDTO?>
    {
        private readonly IPetRepository _petRepository;

        public GetPetByIdQueryHandler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<GetPetDTO?> Handle(
            GetPetByIdQuery request,
            CancellationToken cancellationToken)
        {           
            var pet = await _petRepository.GetByIdAsync(request.Id);
            
            if (pet == null) return null;
           
            var dto = new GetPetDTO
            {
                Id               = pet.Id,
                Name             = pet.PetName,
                Species          = pet.Specie?.SpecieName ?? string.Empty,
                OwnerName        = pet.Owner?.OwnerName ?? string.Empty,
                EmailOwner       = pet.Owner?.Email ?? string.Empty,
                IsActive        = pet.IsActive
            };

            return dto;
        }
    }
}
