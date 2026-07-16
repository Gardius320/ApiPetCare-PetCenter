using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Supplies.Queries.GetAllSupplies
{
    public class PaginatedSuppliesResult
    {
        public List<SupplyDTO> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetAllSuppliesQueryHandler
        : IRequestHandler<GetAllSuppliesQuery, PaginatedSuppliesResult>
    {
        private readonly ISupplyRepository _repository;

        public GetAllSuppliesQueryHandler(ISupplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedSuppliesResult> Handle(
            GetAllSuppliesQuery request, CancellationToken cancellationToken)
        {
            var (supplies, totalRecords) = await _repository.GetAllPagesAsync(
                request.Page, request.PageSize, request.Search, request.CategoryId, request.OnlyActive);

            var items = new List<SupplyDTO>();
            foreach (var s in supplies)
            {
                var dto = new SupplyDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Unit = s.Unit,
                    CurrentStock = s.CurrentStock,
                    MinimumStock = s.MinimumStock,
                    IsActive = s.IsActive,
                    SupplyCategoryId = s.SupplyCategoryId,
                    CategoryName = s.Category.Name
                };
                items.Add(dto);
            }

            return new PaginatedSuppliesResult
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