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
           var owner = await _ownerRepository.GetByIdAsync(request.OwnerId);
            
            if (owner == null) return null;
           
            owner.OwnerName   = request.OwnerName;
            owner.Email       = request.OwnerEmail;
            owner.PhoneNumber = request.OwnerPhone;
            owner.Gender      = request.Gender;
            owner.IdCard      = request.IdCard;
            
            await _ownerRepository.UpdateOwner(owner);
            return request.OwnerId;
        }
    }
}
