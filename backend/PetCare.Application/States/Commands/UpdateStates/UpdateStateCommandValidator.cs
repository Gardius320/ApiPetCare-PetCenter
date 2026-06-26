using FluentValidation;
using MediatR;
using PetCare.Application.States.Commands.UpdateState;



namespace PetCare.Application.States.Commands.UpdateStates
{
    public class UpdateStateCommandValidator : AbstractValidator<UpdateStateCommand>
    {
        public UpdateStateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("El Id es requerido")
                .GreaterThan(0)
                .WithMessage("El Id debe ser mayor que 0.");

            RuleFor(x => x.StateName)
                .NotEmpty()
                .WithMessage("El nombre del estado es requerido")
                .MaximumLength(100)
                .WithMessage("El nombre del estado no puede exceder los 100 caracteres.");
        }
    }
}
