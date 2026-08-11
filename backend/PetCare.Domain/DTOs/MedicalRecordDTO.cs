namespace PetCare.Application.MedicalRecords.DTOs
{
    public class MedicalRecordDTO
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public int? AppointmentId { get; set; }
        public string VeterinarianUserId { get; set; } = string.Empty;
        public string VeterinarianName { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public decimal? Weight { get; set; }
        public decimal? Temperature { get; set; }
        public string? Observations { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
    }
}