using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;
using PetCare.Domain.Enums;

namespace PetCare.Application.Supplies.Commands.UpdateSupply
{
    public class UpdateSupplyCommand : IRequest<ApiResponse<SupplyDTO>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public bool IsActive { get; set; }
        public int SupplyCategoryId { get; set; }
        public SupplyType SupplyType { get; set; }
    }
}
