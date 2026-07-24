using FluentValidation;

namespace PetCare.Application.Supplies.Commands.UpdateSupply
{
    public class UpdateSupplyCommandValidator : AbstractValidator<UpdateSupplyCommand>
    {
        public UpdateSupplyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del insumo no es válido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del insumo es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(150).WithMessage("La descripción no puede superar los 150 caracteres.");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("La unidad de medida es obligatoria.");

            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock actual no puede ser negativo.");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo.");

            RuleFor(x => x.SupplyCategoryId)
                .GreaterThan(0).WithMessage("Debe seleccionar una categoría válida.");

            RuleFor(x => x.SupplyType)
                .IsInEnum().WithMessage("El tipo de insumo debe ser Clínico o Venta.");
        }
    }
}