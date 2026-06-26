using System;
using System.Collections.Generic;

namespace PetCare.Domain.Models;

public partial class Pet
{
    public int Id { get; set; }

    public string PetName { get; set; } = null!;

    public int SpecieId { get; set; }

    public int OwnerId { get; set; }    

    public bool IsActive { get; set; }

    public virtual Owner Owner { get; set; } = null!;

    public virtual Species Specie { get; set; } = null!;


    
}