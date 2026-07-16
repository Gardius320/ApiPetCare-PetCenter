using MediatR;

namespace PetCare.Application.Supplies.Queries.GetAllSupplies
{
    public class GetAllSuppliesQuery : IRequest<PaginatedSuppliesResult>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public bool OnlyActive { get; set; } = true;
    }
}