using MediatR;

namespace PetCare.Application.Pets.Commands.ChangeStatePet
{
    public class ChangeStatePetCommand : IRequest<int?>
    {       
        public int PetId { get; set; }        
        public bool IsActive { get; set; }
    }
}