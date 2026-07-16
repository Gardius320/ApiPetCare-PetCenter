namespace PetCare.Domain.Models
{
    public class SupplyCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
    }
}