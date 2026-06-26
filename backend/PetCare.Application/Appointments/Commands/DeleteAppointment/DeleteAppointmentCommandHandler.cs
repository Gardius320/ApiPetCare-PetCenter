// Handler que cancela una cita usando el repositorio
using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommandHandler
        : IRequestHandler<DeleteAppointmentCommand, int?>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public DeleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<int?> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            // CancelAppointment retorna true si canceló, false si la cita no existía
            var cancelada = await _appointmentRepository.CancelAppointment(request.Id);

            // Si no existía, devolvemos null para que el controlador responda 404
            if (!cancelada) return null;

            return request.Id;
        }
    }
}
