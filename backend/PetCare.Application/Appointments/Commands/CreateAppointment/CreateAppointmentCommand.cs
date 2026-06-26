using MediatR;

namespace PetCare.Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommand : IRequest<int?>
    {
        public int OwnerId { get; set; }
        public int? PetId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Observation { get; set; }
    }
}