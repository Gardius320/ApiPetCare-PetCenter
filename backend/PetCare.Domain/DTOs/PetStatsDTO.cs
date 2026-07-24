namespace PetCare.Domain.DTOs
{
    public class PetStatsDTO
    {
        public SpecieStatsDTO Dogs { get; set; } = new();
        public SpecieStatsDTO Cats { get; set; } = new();
        public SpecieStatsDTO Others { get; set; } = new();
    }

    public class SpecieStatsDTO
    {
        public int Assets { get; set; }
        public int Inactive { get; set; }
    }
}