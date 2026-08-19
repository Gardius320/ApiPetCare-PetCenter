using MediatR;
using PetCare.Application.Supplies.Queries.GetAllSupplies;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Suppliers.Queries.GetAllSuppliers
{
    public class GetAllSuppliersQuery : IRequest<PaginatedSuppliersResult>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public bool OnlyActive { get; set; } = true;
    }
}