using PetCare.Domain.Identity;

namespace PetCare.Domain.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int? AppointmentId { get; set; }
        public string VeterinarianUserId { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;
        public string Diagnopsis { get; set; }
        public string Treatment { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Temperature { get; set; }
        public string? Observation { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public Pet Pet { get; set; } = null!;
        public Appointment? Appointment { get; set; }
        public ApplicationUser Veterinarian { get; set; } = null!;
    }
}
