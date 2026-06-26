using System;
using System.Collections.Generic;

namespace PetCare.Domain.Models;

public partial class Owner
{
    public int Id { get; set; }

    public string OwnerName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public string? Cedula { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
