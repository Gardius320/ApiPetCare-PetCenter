namespace PetCare.Domain.DTOs
{
    public class SupplyStatsDTO
    {
        public int Total { get; set; }
        public int LowStock { get; set; }
        public int OutOfStock { get; set; }
        public int CategoriesInUse { get; set; }
    }
}