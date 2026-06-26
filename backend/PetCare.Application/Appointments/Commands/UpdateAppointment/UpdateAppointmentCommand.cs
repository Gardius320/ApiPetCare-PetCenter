using MediatR;

namespace PetCare.Application.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommand : IRequest<int?>
    {
        
        public int Id { get; set; }
        
        public DateTime AppointmentDate { get; set; }
        
        public string? Observation { get; set; }
    }
}
