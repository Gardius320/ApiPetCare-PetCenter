using System.ComponentModel.DataAnnotations;

namespace PetCare.Domain.DTOs
{
    public class GetPetDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Especie { get; set; }
        public int PropietarioId { get; set; }
        public string? Propietario { get; set; }

        [Display(Name = "Email del Propietario")]
        public string? EmailPropietario { get; set; }

        public string? Estado { get; set; }
    }
}
