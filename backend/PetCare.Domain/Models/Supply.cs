using PetCare.Domain.Enums;

namespace PetCare.Domain.Models
{
    public class Supply
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string Unit { get; set; } = string.Empty;

        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public SupplyType supplyType { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int SupplyCategoryId { get; set; }
        public SupplyCategory Category { get; set; } 

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<SupplyMovement> SupplyMovements { get; set; } = new List<SupplyMovement>();
    }
}