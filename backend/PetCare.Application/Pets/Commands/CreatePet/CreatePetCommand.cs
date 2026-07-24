using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Pets.Commands.CreatePet
{
    public class CreatePetCommand : IRequest<int?>
    {       
        public string? PetName { get; set; }               
        public int SpecieId { get; set; }        
        public int OwnerId { get; set; }
    }
}
