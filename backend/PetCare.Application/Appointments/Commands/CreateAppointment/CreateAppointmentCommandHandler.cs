using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler
        : IRequestHandler<CreateAppointmentCommand, int?>
    {
        // Repositorio que se inyecta por constructor
        private readonly IAppointmentRepository _appointmentRepository;

        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<int?> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
           
            var cita = await _appointmentRepository.CreateAppointment(
                request.OwnerId,
                request.AppointmentDate,
                request.Observation ?? string.Empty,
                request.PetId
            );

            
            return cita?.Id;
        }
    }
}
