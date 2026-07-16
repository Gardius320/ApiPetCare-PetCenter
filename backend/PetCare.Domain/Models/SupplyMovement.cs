using PetCare.Domain.Enums;

namespace PetCare.Domain.Models
{
    public class SupplyMovement
    {
        public int Id { get; set; }
        public int SupplyId { get; set; }
        public Supply Supply { get; set; }

        public MovementType MovementType { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }

        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public DateTime MovementDate { get; set; } = DateTime.UtcNow;
    }
}