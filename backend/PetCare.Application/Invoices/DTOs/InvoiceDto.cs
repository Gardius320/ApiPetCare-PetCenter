using PetCare.Domain.Enums;

namespace PetCare.Application.Invoices.DTOs
{
    public record InvoiceDto(
        Guid Id,
        string InvoiceNumber,
        int OwnerId,
        int? AppointmentId,
        DateTime IssueDate,
        InvoiceStatus Status,
        decimal Subtotal,
        decimal Tax,
        decimal Total,
        PaymentMethod? PaymentMethod,
        string? PaymentReference,
        DateTime? PaymentDate
    );
}