using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Domain.DTOs
{
    public class GetSpeciesDTO
    {
        public int Id { get; set; }
        public string? SpeciesName { get; set; }
    }
}
