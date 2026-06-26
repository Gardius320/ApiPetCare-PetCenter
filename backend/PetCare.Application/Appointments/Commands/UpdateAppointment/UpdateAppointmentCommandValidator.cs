using FluentValidation;
using MediatR;



namespace PetCare.Application.Appointments.Commands.UpdateAppointment
{
    public  class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator() 
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar una cita válida");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .WithMessage("La fecha de la cita es obligatoria")
                .GreaterThan(DateTime.Now)
                .WithMessage("La fecha de la cita debe ser en el futuro");

            RuleFor(x => x.Observation)
                .MaximumLength(500)
                .MinimumLength(5)
                .WithMessage("La observación debe tener entre 5 y 500 caracteres");
        }

    }
}
