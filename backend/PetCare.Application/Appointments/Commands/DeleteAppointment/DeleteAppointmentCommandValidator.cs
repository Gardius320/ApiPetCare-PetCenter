using FluentValidation;
using MediatR;


namespace PetCare.Application.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommandValidator : AbstractValidator<DeleteAppointmentCommand>
    {

        public DeleteAppointmentCommandValidator() 
        {
            // El Id de la cita a eliminar debe ser un número positivo
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Debes proporcionar un Id de cita válido para eliminar");
        }

    }
}
