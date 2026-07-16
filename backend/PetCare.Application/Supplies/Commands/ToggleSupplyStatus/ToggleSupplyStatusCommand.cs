using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Supplies.Commands.ToggleSupplyStatus
{
    public class ToggleSupplyStatusCommand : IRequest<ApiResponse<SupplyDTO>>
    {
        public int Id { get; set; }
    }
}
