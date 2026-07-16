using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerCommandHandler 
        : IRequestHandler<DeleteOwnerCommand, int?>
    {
        private readonly IOwnerRepository _ownerRepository;

        public DeleteOwnerCommandHandler(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<int?> Handle(DeleteOwnerCommand command, CancellationToken cancellationToken)
        {
            
            var owner = await _ownerRepository.GetByIdAsync(command.Id);
            
            if (owner is null) return null;
            
            await _ownerRepository.DeleteOwner(command.Id);
            return owner.Id; 
        }
    }
}
