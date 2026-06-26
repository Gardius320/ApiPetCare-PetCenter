// Query para obtener los dueños con soporte de paginación y búsqueda
using MediatR;

namespace PetCare.Application.Owners.Queries.GetAllOwners
{
    public class GetAllOwnersQuery : IRequest<PaginatedOwnersResult>
    {
        // Página actual
        public int Page { get; set; } = 1;

        // Cantidad de registros por página
        public int PageSize { get; set; } = 10;

        // Filtro de búsqueda opcional
        public string? Search { get; set; }
    }
}
