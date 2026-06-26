// Handler que busca una mascota por Id y la convierte a DTO
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
            // Buscamos la mascota en la base de datos
            var pet = await _petRepository.GetByIdAsync(request.Id);

            // Si no existe, devolvemos null para que el controlador responda 404
            if (pet == null) return null;

            // Construimos el DTO con los datos de la mascota
            var dto = new GetPetDTO
            {
                Id               = pet.Id,
                Nombre           = pet.PetName,
                Especie          = pet.Specie?.SpecieName ?? string.Empty,
                Propietario      = pet.Owner?.OwnerName ?? string.Empty,
                EmailPropietario = pet.Owner?.Email ?? string.Empty,
                Estado           = pet.IsActive ? "Activo" : "Inactivo"
            };

            return dto;
        }
    }
}
