using MediatR;

namespace PetCare.Application.Owners.Commands.UpdateOwner
{
    public class UpdateOwnerCommand : IRequest<int?>
    {
        
        public int OwnerId { get; set; }

        public string? OwnerName { get; set; }
        public string? OwnerEmail { get; set; }
        public string? OwnerPhone { get; set; }
        public string? Gender { get; set; }
        public string? IdCard { get; set; }
    }
}
