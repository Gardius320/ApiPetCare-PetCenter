using FluentValidation;

namespace PetCare.Application.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerCommandValidator : AbstractValidator<DeleteOwnerCommand>
    {
        public DeleteOwnerCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("El Id es requerido")
                .GreaterThan(0)
                .WithMessage("El Id debe ser mayor que 0.");
        }
    }
}
