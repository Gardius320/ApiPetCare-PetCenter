using MediatR;

namespace PetCare.Application.Owners.Queries.GetAllOwners
{
    public class GetAllOwnersQuery : IRequest<PaginatedOwnersResult>
    {        
        public int Page { get; set; } = 1;
        
        public int PageSize { get; set; } = 10;
        
        public string? Search { get; set; }
    }
}
