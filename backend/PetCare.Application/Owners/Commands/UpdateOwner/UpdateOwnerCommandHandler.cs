// Handler que busca el dueño, le actualiza los datos y lo guarda
using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Owners.Commands.UpdateOwner
{
    public class UpdateOwnerCommandHandler
        : IRequestHandler<UpdateOwnerCommand, int?>
    {
        private readonly IOwnerRepository _ownerRepository;

        public UpdateOwnerCommandHandler(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<int?> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            // Buscamos el dueño en la base de datos
            var dueno = await _ownerRepository.GetByIdAsync(request.OwnerId);

            // Si no existe, devolvemos null para que el controlador responda 404
            if (dueno == null) return null;

            // Actualizamos los campos con los datos nuevos
            dueno.OwnerName   = request.OwnerName;
            dueno.Email       = request.OwnerEmail;
            dueno.PhoneNumber = request.OwnerPhone;
            dueno.Gender      = request.Gender;
            dueno.Cedula      = request.Cedula;

            // Guardamos los cambios
            await _ownerRepository.UpdateOwner(dueno);
            return request.OwnerId;
        }
    }
}
