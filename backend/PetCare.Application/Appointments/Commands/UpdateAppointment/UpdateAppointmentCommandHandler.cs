// Handler que actualiza los datos de una cita existente
using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler
        : IRequestHandler<UpdateAppointmentCommand, int?>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public UpdateAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<int?> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Intentamos actualizar; el repo devuelve false si la cita no existe
            var actualizada = await _appointmentRepository.UpdateAppointment(
                request.Id,
                request.AppointmentDate,
                request.Observation ?? string.Empty
            );

            // Si no se encontró la cita, avisamos con null
            if (!actualizada) return null;

            return request.Id;
        }
    }
}
