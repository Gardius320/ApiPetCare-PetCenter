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
            var owner = new PetCare.Domain.Models.Owner
            {
                OwnerName   = request.OwnerName,
                Email       = request.Email,
                PhoneNumber = request.PhoneNumber,
                IdCard      = request.IdCard,
                Gender      = request.Gender
            };
            
            var createdOwner = await _ownerRepository.CreateOwner(owner);
            return createdOwner?.Id;
        }
    }
}