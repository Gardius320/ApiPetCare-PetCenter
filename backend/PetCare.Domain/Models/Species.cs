using System;
using System.Collections.Generic;

namespace PetCare.Domain.Models;

public partial class Species
{
    public int Id { get; set; }

    public string SpecieName { get; set; } = null!;

    public byte[] CreateAt { get; set; } = null!;   


   
}
