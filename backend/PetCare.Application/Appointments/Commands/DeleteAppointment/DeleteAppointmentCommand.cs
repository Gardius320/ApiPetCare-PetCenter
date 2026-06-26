using MediatR;

namespace PetCare.Application.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommand : IRequest<int?>
    {
        // Id de la cita que se quiere cancelar
        public int Id { get; set; }
    }
}
