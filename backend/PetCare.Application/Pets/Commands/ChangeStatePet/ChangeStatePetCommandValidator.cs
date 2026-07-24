using FluentValidation;
using MediatR;

namespace PetCare.Application.Pets.Commands.ChangeStatePet
{
    public class ChangeStatePetCommandValidator : AbstractValidator<ChangeStatePetCommand>
    {
        public ChangeStatePetCommandValidator() 
        {

            RuleFor(x => x.PetId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar una mascota válida");

            RuleFor(x => x.IsActive)
                .NotNull()
                .WithMessage("Debes seleccionar un estado válido");
        }
    }
}
