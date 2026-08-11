using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Appointments.Queries.GetBillableAppointments
{
    public record GetBillableAppointmentsQuery(int OwnerId) : IRequest<List<BillableAppointmentDto>>;
}
