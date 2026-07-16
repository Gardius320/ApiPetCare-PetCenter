using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Supplies.Commands.ToggleSupplyStatus
{
    public class ToggleSupplyStatusCommandHandler : IRequestHandler<ToggleSupplyStatusCommand, ApiResponse<SupplyDTO>>
    {
        private readonly ISupplyRepository _supplyRepository;

        public ToggleSupplyStatusCommandHandler(ISupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }

        public async Task<ApiResponse<SupplyDTO>> Handle(ToggleSupplyStatusCommand request, CancellationToken cancellationToken)
        {
            var supply = await _supplyRepository.GetByIdAsync(request.Id);

            if (supply == null)
                return ApiResponse<SupplyDTO>.Failure("Insumo no encontrado");

            supply.IsActive = !supply.IsActive;

            var updated = await _supplyRepository.UpdateSupply(supply);

            if (updated == null)
                return ApiResponse<SupplyDTO>.Failure("No se pudo cambiar el estado del insumo");

            var dto = new SupplyDTO
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description,
                Unit = updated.Unit,
                CurrentStock = updated.CurrentStock,
                MinimumStock = updated.MinimumStock,
                IsActive = updated.IsActive,
                SupplyCategoryId = updated.SupplyCategoryId,
                CategoryName = updated.Category?.Name ?? string.Empty
            };

            var mensaje = updated.IsActive ? "Insumo activado correctamente" : "Insumo desactivado correctamente";
            return ApiResponse<SupplyDTO>.Success(dto, mensaje);
        }
    }
}
