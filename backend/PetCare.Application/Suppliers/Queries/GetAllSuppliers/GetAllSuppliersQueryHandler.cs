using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Suppliers.Queries.GetAllSuppliers
{
    public class PaginatedSuppliersResult
    {
        public List<SupplierDTO> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetAllSuppliersQueryHandler
        : IRequestHandler<GetAllSuppliersQuery, PaginatedSuppliersResult>
    {
        private readonly ISupplierRepository _repository;

        public GetAllSuppliersQueryHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedSuppliersResult> Handle(
            GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var (suppliers, totalRecords) = await _repository.GetAllPagesAsync(
                request.Page, request.PageSize, request.Search, request.OnlyActive);

            var items = new List<SupplierDTO>();
            foreach (var s in suppliers)
            {
                items.Add(new SupplierDTO
                {
                    Id = s.Id,
                    Name = s.Name ?? string.Empty,
                    ContactNumber = s.ContactNumber,
                    Email = s.Email,
                    Address = s.Address,
                    Description = s.Description,
                    IsActive = s.IsActive
                });
            }

            return new PaginatedSuppliersResult
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