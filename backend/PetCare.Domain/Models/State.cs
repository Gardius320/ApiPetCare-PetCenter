namespace PetCare.Domain.Models;

public partial class State
{
    public int IdState { get; set; }

    public string StateName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
