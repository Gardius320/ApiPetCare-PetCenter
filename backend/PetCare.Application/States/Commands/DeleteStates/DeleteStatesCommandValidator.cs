using MediatR;
using FluentValidation;

namespace PetCare.Application.States.Commands.DeleteStates
{
    public class DeleteStatesCommandValidator : AbstractValidator<DeleteStatesCommand>
    {

        public DeleteStatesCommandValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("El Id es requerido")
                .GreaterThan(0)
                .WithMessage("El Id debe ser mayor que 0.");

        }
    }
}

