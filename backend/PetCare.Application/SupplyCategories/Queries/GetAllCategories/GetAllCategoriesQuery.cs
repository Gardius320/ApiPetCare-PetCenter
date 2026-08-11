using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.SupplyCategories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<PaginatedCategoriesResult>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public bool OnlyActive { get; set; } = true;
    
    }
}
