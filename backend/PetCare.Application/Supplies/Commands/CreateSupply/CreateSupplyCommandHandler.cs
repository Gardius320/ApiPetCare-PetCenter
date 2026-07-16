using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Supplies.Commands.CreateSupply
{
    public class CreateSupplyCommandHandler : IRequestHandler<CreateSupplyCommand, ApiResponse<string>>
    {
        private readonly ISupplyRepository _supplyRepository;

        public CreateSupplyCommandHandler(ISupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }

        public async Task<ApiResponse<string>> Handle(CreateSupplyCommand request, CancellationToken cancellationToken)
        {
            var supply = new Supply
            {
                Name = request.Name,
                Description = request.Description,
                Unit = request.Unit,
                CurrentStock = request.CurrentStock,
                MinimumStock = request.MinimumStock,
                SupplyCategoryId = request.SupplyCategoryId
            };

            var created = await _supplyRepository.CreateSupply(supply);

            if (created == null)
                return ApiResponse<string>.Failure("No se pudo crear el insumo");

            return ApiResponse<string>.Success("Insumo creado correctamente");
        }
    }
}