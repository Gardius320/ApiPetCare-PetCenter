using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.SupplyCategories.Queries.GetAllCategories
{
    public class PaginatedCategoriesResult
    {
        public List<SupplyCategoryDTO> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetAllCategoriesQueryHandler
        : IRequestHandler<GetAllCategoriesQuery, PaginatedCategoriesResult>
    {
        private readonly ISupplyCategoryRepository _repository;

        public GetAllCategoriesQueryHandler(ISupplyCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedCategoriesResult> Handle(
            GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var (categories, totalRecords) = await _repository.GetAllPagesAsync(
                request.Page, request.PageSize, request.Search, request.OnlyActive);

            var items = new List<SupplyCategoryDTO>();
            foreach (var c in categories)
            {
                items.Add(new SupplyCategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive
                });
            }

            return new PaginatedCategoriesResult
            {
                Items = items,
                TotalRecords = totalRecords,
                TotalPages = totalRecords > 0
                    ? (int)Math.Ceiling((double)totalRecords / request.PageSize)
                    : 1
            };
        }
    }
}
