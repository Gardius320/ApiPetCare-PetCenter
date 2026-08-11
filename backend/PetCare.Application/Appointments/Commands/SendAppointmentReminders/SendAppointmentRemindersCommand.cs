using MediatR;

namespace PetCare.Application.Appointments.Commands.SendAppointmentReminders
{
    public record SendAppointmentRemindersCommand : IRequest<int>;
}