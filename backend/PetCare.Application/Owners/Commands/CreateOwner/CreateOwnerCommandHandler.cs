// Handler que arma el objeto Owner y lo guarda en la base de datos
using MediatR;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommandHandler
        : IRequestHandler<CreateOwnerCommand, int?>
    {
        private readonly IOwnerRepository _ownerRepository;

        public CreateOwnerCommandHandler(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<int?> Handle(
            CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            // Creamos el objeto de dominio con los datos del comando
            var owner = new PetCare.Domain.Models.Owner
            {
                OwnerName   = request.OwnerName,
                Email       = request.Email,
                PhoneNumber = request.PhoneNumber,
                Cedula      = request.Cedula,
                Gender      = request.Gender
            };

            // Lo guardamos y devolvemos el Id generado
            var createdOwner = await _ownerRepository.CreateOwner(owner);
            return createdOwner?.Id;
        }
    }
}