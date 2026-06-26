namespace PetCare.Domain.DTOs
{
    public class OwnerDTO
    {
        public int Id { get; set; }
        public string? OwnerName { get; set; }   // serializa como "ownerName"
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; } // serializa como "phoneNumber"
        public string? Cedula { get; set; }
        public string? Sexo { get; set; }        // serializa como "sexo"
    }
}
