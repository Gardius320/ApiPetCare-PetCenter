using MediatR;
using FluentValidation;



namespace PetCare.Application.Species.Commands.CreateSpecies
{
    public class CreateSpeciesCommandsValidator : AbstractValidator<CreateSpeciesCommand>
    {

        public CreateSpeciesCommandsValidator() 
        {
            RuleFor(x => x.SpecieName)
                .NotEmpty()
                .WithMessage("El nombre de la especie es requerido.")
                .MaximumLength(100)
                .WithMessage("El nombre de la especie no puede exceder los 100 caracteres.");
        }
    }
}
