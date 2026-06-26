using MediatR;
using FluentValidation;


namespace PetCare.Application.Pets.Commands.CreatePet
{
    public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
    {

        public CreatePetCommandValidator() 
        {

            RuleFor(x => x.PetName)
                .NotEmpty()
                .WithMessage("El nombre de la mascota es obligatorio")
                .MaximumLength(50)
                .WithMessage("El nombre no puede exceder los 50 caracteres");

            RuleFor(x => x.SpecieId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar una especie válida");

            RuleFor(x => x.OwnerId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar un dueño válido");

        }
    }
}
