namespace PetCare.Domain.DTOs
{
    public class SupplyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public bool IsActive { get; set; }
        public int SupplyCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}