using MediatR;

namespace PetCare.Application.Appointments.Queries.GetAllAppointments
{
    public class GetAllAppointmentsQuery : IRequest<PaginatedAppointmentsResult>
    {        
        public int Page { get; set; } = 1;        
        public int PageSize { get; set; } = 10;        
        public string? Search { get; set; }
    }
}
