using MediatR;
using FluentValidation;

namespace PetCare.Application.States.Commands.CreateStates
{
    public class CreateStatesCommandValidator : AbstractValidator<CreateStatesCommand>
    {

        public CreateStatesCommandValidator() 
        {
            RuleFor(x => x.StateName)
                .NotEmpty()
                .WithMessage("El nombre del estado es requerido")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("El estado debe tener una descripción y no puede exceder los 500 caracteres");


        }
    }
}
