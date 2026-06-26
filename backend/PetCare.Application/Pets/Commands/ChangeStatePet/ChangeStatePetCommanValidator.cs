using FluentValidation;
using MediatR;

namespace PetCare.Application.Pets.Commands.ChangeStatePet
{
    public class ChangeStatePetCommanValidator : AbstractValidator<ChangeStatePetCommand>
    {
        public ChangeStatePetCommanValidator() 
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
