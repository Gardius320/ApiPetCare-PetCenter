using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.ChangeAppointmentState
{
    public class ChangeAppointmentStateCommandHandler : IRequestHandler<ChangeAppointmentStateCommand, bool>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public ChangeAppointmentStateCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> Handle(ChangeAppointmentStateCommand request, CancellationToken cancellationToken)
        {
            return await _appointmentRepository.ChangeAppointmentState(request.Id, request.StateId);
        }
    }
}