using MediatR;
using PetCare.Application.Users.Queries.GetUsersByRole;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Queries.GetAvailableSlots
{
    public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, List<string>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMediator _mediator;

        public GetAvailableSlotsQueryHandler(
            IAppointmentRepository appointmentRepository,
            IMediator mediator)
        {
            _appointmentRepository = appointmentRepository;
            _mediator = mediator;
        }

        public async Task<List<string>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
          
            var vetsResponse = await _mediator.Send(
                new GetUsersByRoleQuery { Role = "Veterinarian" },
                cancellationToken);

            int vetCount = vetsResponse.Data?.Count ?? 0;

          
            var appointments = await _appointmentRepository.GetActiveAppointmentsForDateAsync(request.Date);

           
            var availableSlots = new List<string>();

            for (int hora = 8; hora <= 17; hora++)
            {
                int occupied = appointments.Count(a => a.AppointmentDate.Hour == hora);

                if (occupied < vetCount)
                {
                    availableSlots.Add($"{hora:D2}:00");
                }
            }
          
            return availableSlots;
        }
    }
}