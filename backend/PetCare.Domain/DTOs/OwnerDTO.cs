namespace PetCare.Domain.DTOs
{
    public class OwnerDTO
    {
        public int Id { get; set; }
        public string? OwnerName { get; set; }   
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; } 
        public string? IdCard{ get; set; }
        public string? Gender { get; set; }        
    }
}
