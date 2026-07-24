using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler
        : IRequestHandler<CreateAppointmentCommand, int?>
    {        
        private readonly IAppointmentRepository _appointmentRepository;

        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<int?> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
           
            var appointment = await _appointmentRepository.CreateAppointment(
                request.OwnerId,
                request.AppointmentDate,
                request.Observation ?? string.Empty,
                request.PetId
            );

            
            return appointment?.Id;
        }
    }
}
