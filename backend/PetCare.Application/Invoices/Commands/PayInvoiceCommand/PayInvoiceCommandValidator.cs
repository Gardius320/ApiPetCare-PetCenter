using FluentValidation;

namespace PetCare.Application.Invoices.Commands.PayInvoice
{
    public class PayInvoiceCommandValidator : AbstractValidator<PayInvoiceCommand>
    {
        public PayInvoiceCommandValidator()
        {
            RuleFor(x => x.InvoiceId).NotEmpty().WithMessage("El id de la factura es obligatorio.");
            RuleFor(x => x.PaymentMethod).IsInEnum().WithMessage("El método de pago no es válido.");
        }
    }
}