namespace PetCare.Domain.DTOs
{
    public class PetStatsDTO
    {
        public SpecieStatsDTO Perros { get; set; } = new();
        public SpecieStatsDTO Gatos { get; set; } = new();
        public SpecieStatsDTO Otros { get; set; } = new();
    }

    public class SpecieStatsDTO
    {
        public int Activos { get; set; }
        public int Inactivos { get; set; }
    }
}