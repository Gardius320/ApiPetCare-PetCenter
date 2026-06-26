// Handler que busca la mascota, actualiza sus datos y los guarda
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
            // Buscamos la mascota por Id
            var pet = await _petRepository.GetByIdAsync(request.PetId);

            // Si no existe, devolvemos null para que el controlador responda 404
            if (pet == null) return null;

            // Sobreescribimos los campos con los valores nuevos
            pet.PetName  = request.PetName;
            pet.SpecieId = request.SpecieId;
            pet.OwnerId  = request.OwnerId;

            // Guardamos los cambios
            await _petRepository.UpdatePet(request.PetId, pet);
            return request.PetId;
        }
    }
}