using PetCare.Domain.Enums;

namespace PetCare.Application.Invoices.DTOs
{
    public record InvoiceItemDto(
        Guid Id,
        InvoiceItemType ItemType,
        string Description,
        int? SupplyId,
        int Quantity,
        decimal UnitPrice,
        decimal LineTotal
    );
}
