using MediatR;


namespace PetCare.Application.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommand : IRequest<int?>
    {
      
        public string? OwnerName { get; set; }       
        
        public string? Email { get; set; }
        
        public string? PhoneNumber { get; set; }        
        
        public string? IdCard { get; set; }
       
        public string? Gender { get; set; }
    }
}