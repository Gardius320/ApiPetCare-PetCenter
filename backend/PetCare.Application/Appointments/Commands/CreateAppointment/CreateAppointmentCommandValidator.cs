using FluentValidation;
using MediatR;

namespace PetCare.Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator() 
        {
            //El Dueño debe ser un Id real y nunca un 0 o un negativo
            RuleFor(x => x.OwnerId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar un dueño válido");

            //de la misma forma que el owner  pero con el petId
            RuleFor(x => x.PetId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar una mascota válida")
                .When(x => x.PetId.HasValue);
            
            //Acá valido la fecha que noe ste vacia ni que sea menor o futura
            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .WithMessage("La fecha de la cita es obligatoria")
                .GreaterThan(DateTime.Now)
                .WithMessage("La fecha de la cita debe ser en el futuro");


            //// Observation es opcional, pero si viene no puede superar 500 caracteres
            RuleFor(x => x.Observation)
                .MaximumLength(500)
                .WithMessage("La observación no puede tener más de 500 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Observation));

        }
    }
}
