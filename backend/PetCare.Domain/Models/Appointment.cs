namespace PetCare.Domain.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Observation { get; set; }

    public int OwnerId { get; set; }

    public byte[] CreateAt { get; set; } = null!;

    public int StateId { get; set; }
    public int? PetId { get; set; }
    public virtual Pet? Pet { get; set; }

    public virtual Owner Owner { get; set; } = null!;

    public virtual State State { get; set; } = null!;
}
