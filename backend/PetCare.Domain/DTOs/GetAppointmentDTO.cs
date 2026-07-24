namespace PetCare.Domain.DTOs
{
    public class GetAppointmentDTO
    {
        public int Id { get; set; }
        public string? Date { get; set; }
        public string? OwnerName { get; set; }
        public string? State { get; set; }
        public string? Observation { get; set; }
        public string? PetName { get; set; }
        public string? Species { get; set; }
    }
}
