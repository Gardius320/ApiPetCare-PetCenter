using MediatR;
using FluentValidation;



namespace PetCare.Application.Pets.Commands.UpdatePet
{
    public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
    {

        public UpdatePetCommandValidator() 
        {
            RuleFor(x => x.PetId)
                .GreaterThan(0)
                .WithMessage("El ID de la mascota es requerido.");

            RuleFor(x => x.PetName)
                .MaximumLength(100)
                .WithMessage("El nombre de la mascota no puede exceder los 100 caracteres.");

            RuleFor(x => x.SpecieId)
                .GreaterThan(0)
                .WithMessage("El ID de la especie es requerido.");

            RuleFor(x => x.OwnerId)
                .GreaterThan(0)
                .WithMessage("El ID del dueño es requerido.");
        }
    }
}
