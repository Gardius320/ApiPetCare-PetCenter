using MediatR;

namespace PetCare.Application.Appointments.Commands.ChangeAppointmentState
{
    public class ChangeAppointmentStateCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int StateId { get; set; }
    }
}