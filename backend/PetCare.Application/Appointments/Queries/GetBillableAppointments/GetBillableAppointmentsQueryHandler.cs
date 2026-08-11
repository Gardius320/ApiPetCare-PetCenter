using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Queries.GetBillableAppointments
{
    public class GetBillableAppointmentsQueryHandler
        : IRequestHandler<GetBillableAppointmentsQuery, List<BillableAppointmentDto>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetBillableAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<BillableAppointmentDto>> Handle(
            GetBillableAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetBillableAppointmentsAsync(request.OwnerId);

            return appointments.Select(a => new BillableAppointmentDto(
                a.Id,
                a.AppointmentDate,
                a.Pet?.PetName ?? "Sin mascota asociada"
            )).ToList();
        }
    }
}
