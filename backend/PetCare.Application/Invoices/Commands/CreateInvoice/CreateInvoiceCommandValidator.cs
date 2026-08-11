using FluentValidation;
using PetCare.Domain.Enums;

namespace PetCare.Application.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator()
        {
            RuleFor(x => x.OwnerId).GreaterThan(0).WithMessage("El dueño es obligatorio.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("La factura debe tener al menos un ítem.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
                item.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("El precio unitario no puede ser negativo.");

                item.RuleFor(i => i.SupplyId)
                    .NotNull().WithMessage("Un ítem de tipo Supply requiere SupplyId.")
                    .When(i => i.ItemType == InvoiceItemType.Supply);

                item.RuleFor(i => i.Description)
                    .NotEmpty().WithMessage("La descripción es obligatoria.")
                    .When(i => i.ItemType == InvoiceItemType.Service);
            });
        }
    }
}
