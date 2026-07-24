using MediatR;
using PetCare.Application.Pets.Queries.GetAllPets;

namespace PetCare.Application.Pets.Queries.GetAllPets
{
    public class GetAllPetsQuery : IRequest<PaginatedPetsResult>
    {
        
        public int Page { get; set; } = 1;
      
        public int PageSize { get; set; } = 10;
      
        public string? Search { get; set; }
    }
}
