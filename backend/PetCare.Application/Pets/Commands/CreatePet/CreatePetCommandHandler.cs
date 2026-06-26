// Handler que crea el objeto Pet y lo persiste en la base de datos
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
            // Armamos el objeto mascota con los datos del formulario
            var mascota = new Pet
            {
                PetName  = request.PetName ?? string.Empty,
                SpecieId = request.SpecieId,
                OwnerId  = request.OwnerId,
                IsActive = true // toda mascota nueva empieza activa
            };

            // Guardamos en la base de datos
            var creada = await _petRepository.CreatePetAsync(mascota);

            // Si el dueño no existe, el repo devuelve null
            return creada?.Id;
        }
    }
}
