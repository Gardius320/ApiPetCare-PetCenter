using MediatR;
using PetCare.Application.Common;

namespace PetCare.Application.Supplies.Commands.CreateSupply
{
    public class CreateSupplyCommand : IRequest<ApiResponse<string>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public int SupplyCategoryId { get; set; }
    }
}