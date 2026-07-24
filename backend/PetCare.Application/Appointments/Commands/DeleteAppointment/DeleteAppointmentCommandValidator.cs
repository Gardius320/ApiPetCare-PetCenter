using FluentValidation;
using MediatR;


namespace PetCare.Application.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommandValidator : AbstractValidator<DeleteAppointmentCommand>
    {

        public DeleteAppointmentCommandValidator() 
        {           
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Debes proporcionar un Id de cita válido para eliminar");
        }

    }
}
