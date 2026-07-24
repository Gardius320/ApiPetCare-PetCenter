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
            var cancelled = await _appointmentRepository.CancelAppointment(request.Id);
           
            if (!cancelled) return null;

            return request.Id;
        }
    }
}
