using MediatR;

namespace PetCare.Application.Pets.Commands.UpdatePet
{
    public class UpdatePetCommand : IRequest<int?>
    {
        
        public int PetId { get; set; }
        
        public string? PetName { get; set; }
       
        public int SpecieId { get; set; }
       
        public int OwnerId { get; set; }
    }
}