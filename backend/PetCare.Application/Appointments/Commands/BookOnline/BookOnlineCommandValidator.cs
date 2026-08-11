using FluentValidation;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.BookOnline
{
    public class BookOnlineCommandValidator : AbstractValidator<BookOnlineCommand>
    {
        public BookOnlineCommandValidator(ISpeciesRepository speciesRepository)
        {
            RuleFor(x => x.OwnerName)
                .NotEmpty()
                .WithMessage("El nombre es obligatorio");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("El correo electrónico es obligatorio y debe ser válido");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(10)
                .WithMessage("El número de teléfono debe tener los caracteres correspondientes");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .WithMessage("La fecha de la cita es obligatoria")
                .GreaterThan(DateTime.Now)
                .WithMessage("La fecha de la cita debe ser en el futuro");

            RuleFor(x => x.Observation)
                .MaximumLength(500)
                .WithMessage("La observación no puede tener más de 500 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Observation));

            RuleFor(x => x.PetName)
                .NotEmpty()
                .WithMessage("El nombre de la mascota es obligatorio")
                .MaximumLength(100)
                .WithMessage("El nombre de la mascota no puede tener más de 100 caracteres");

            RuleFor(x => x.SpecieId)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar una especie")
                .MustAsync(async (specieId, cancellation) =>
                {
                    var species = await speciesRepository.GetAll();
                    return species.Any(s => s.Id == specieId);
                })
                .WithMessage("La especie seleccionada no existe");
        }
    }
}