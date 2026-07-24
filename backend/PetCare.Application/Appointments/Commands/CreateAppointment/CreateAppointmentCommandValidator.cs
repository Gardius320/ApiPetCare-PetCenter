using FluentValidation;
using MediatR;

namespace PetCare.Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator() 
        {            
            RuleFor(x => x.OwnerId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar un dueño válido");

            
            RuleFor(x => x.PetId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar una mascota válida")
                .When(x => x.PetId.HasValue);
            
            
            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .WithMessage("La fecha de la cita es obligatoria")
                .GreaterThan(DateTime.Now)
                .WithMessage("La fecha de la cita debe ser en el futuro");
           
            RuleFor(x => x.Observation)
                .MaximumLength(500)
                .WithMessage("La observación no puede tener más de 500 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Observation));

        }
    }
}
