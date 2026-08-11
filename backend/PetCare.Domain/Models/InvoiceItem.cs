using PetCare.Domain.Enums;

namespace PetCare.Domain.Models
{
    public class InvoiceItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid InvoiceId { get; set; }

        public InvoiceItemType ItemType { get; set; }
        public string Description { get; set; } = string.Empty;

        public int? SupplyId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}