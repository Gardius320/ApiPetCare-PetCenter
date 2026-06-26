namespace PetCare.Domain.DTOs
{
    public class GetAppointmentDTO
    {
        public int Id { get; set; }
        public string? Fecha { get; set; } 
        public string? NombreDueno { get; set; }
        public string? Estado { get; set; } 
        public string? Observacion { get; set; }
        public string? NombreMascota { get; set; }
        public string? Especie { get; set; }
    }
}
