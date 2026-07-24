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
            var updated = await _appointmentRepository.UpdateAppointment(
                request.Id,
                request.AppointmentDate,
                request.Observation ?? string.Empty
            );
            
            if (!updated) return null;

            return request.Id;
        }
    }
}
