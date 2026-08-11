namespace PetCare.Domain.Enums
{
    public enum InvoiceStatus
    {
        Pending = 0,
        Paid = 1,
        Cancelled = 2
    }
    public enum PaymentMethod
    {
        Cash = 0,
        Transfer = 1,
        Card = 2
    }

    public enum InvoiceItemType
    {
        Service = 0,
        Supply = 1
    }
}