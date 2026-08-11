namespace PetCare.Application.Invoices.DTOs
{
    public record InvoiceDetailDto(InvoiceDto Invoice, List<InvoiceItemDto> Items);
}
