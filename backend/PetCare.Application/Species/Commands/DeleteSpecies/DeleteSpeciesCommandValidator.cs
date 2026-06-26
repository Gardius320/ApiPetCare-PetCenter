using MediatR;
using FluentValidation;

namespace PetCare.Application.Species.Commands.DeleteSpecies
{
    public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
    {
        public DeleteSpeciesCommandValidator()
        {
            
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El ID de la especie es requerido.");
        }
    }
}
