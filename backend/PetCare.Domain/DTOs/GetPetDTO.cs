using System.ComponentModel.DataAnnotations;

namespace PetCare.Domain.DTOs
{
    public class GetPetDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Species { get; set; }
        public int OwnerId { get; set; }
        public string? OwnerName { get; set; }

        [Display(Name = "Email del Propietario")]
        public string? EmailOwner { get; set; }

        public bool IsActive { get; set; }
    }
}
