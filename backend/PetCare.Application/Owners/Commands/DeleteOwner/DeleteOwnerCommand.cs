using MediatR;

namespace PetCare.Application.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerCommand : IRequest <int?>
    {
        public int Id { get; set; }
    
    }
}
