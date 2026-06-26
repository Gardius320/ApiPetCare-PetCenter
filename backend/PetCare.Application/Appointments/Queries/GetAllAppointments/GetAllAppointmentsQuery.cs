// Query para pedir la lista de citas con paginación y búsqueda
using MediatR;

namespace PetCare.Application.Appointments.Queries.GetAllAppointments
{
    public class GetAllAppointmentsQuery : IRequest<PaginatedAppointmentsResult>
    {
        // Página actual, empieza en 1
        public int Page { get; set; } = 1;

        // Cuántas citas traer por página
        public int PageSize { get; set; } = 10;

        // Texto para filtrar (opcional)
        public string? Search { get; set; }
    }
}
