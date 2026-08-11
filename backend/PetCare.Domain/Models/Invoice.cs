using PetCare.Domain.Enums;

namespace PetCare.Domain.Models
{
    public class Invoice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime IssueDate { get; set; }
        public InvoiceStatus Status { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public string? PaymentReference { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string CreatedByUserId { get; set; } = string.Empty;

        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}