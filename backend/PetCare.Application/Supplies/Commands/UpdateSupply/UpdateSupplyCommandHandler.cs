using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Supplies.Commands.UpdateSupply
{
    public class UpdateSupplyCommandHandler : IRequestHandler<UpdateSupplyCommand, ApiResponse<SupplyDTO>>
    {
        private readonly ISupplyRepository _supplyRepository;

        public UpdateSupplyCommandHandler(ISupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }

        public async Task<ApiResponse<SupplyDTO>> Handle(UpdateSupplyCommand request, CancellationToken cancellationToken)
        {
            var supply = await _supplyRepository.GetByIdAsync(request.Id);

            if (supply == null)
                return ApiResponse<SupplyDTO>.Failure("Insumo no encontrado");

            supply.Name = request.Name;
            supply.Description = request.Description;
            supply.Unit = request.Unit;
            supply.CurrentStock = request.CurrentStock;
            supply.MinimumStock = request.MinimumStock;
            supply.IsActive = request.IsActive;
            supply.SupplyCategoryId = request.SupplyCategoryId;
            supply.supplyType = request.SupplyType;

            var updated = await _supplyRepository.UpdateSupply(supply);

            if (updated == null)
                return ApiResponse<SupplyDTO>.Failure("No se pudo actualizar el insumo");

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
                CategoryName = updated.Category?.Name ?? string.Empty,
                SupplyType = updated.supplyType
            };

            return ApiResponse<SupplyDTO>.Success(dto, "Insumo actualizado correctamente");
        }
    }
}