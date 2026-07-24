using FluentValidation;
using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.SupplyCategories.Commands.Create
{ 
    public class CreateCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

    public class CreateCommandHandler : IRequestHandler<CreateCommand, int>
    {
        private readonly ISupplyCategoryRepository _supplyCategoryRepository;

        public CreateCommandHandler(ISupplyCategoryRepository supplyCategoryRepository)
        {
            _supplyCategoryRepository = supplyCategoryRepository;
        }

        public async Task<int> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var supplyCategory = new SupplyCategory
            {
                Name = request.Name,
                Description = request.Description
            };

            await _supplyCategoryRepository.Create(supplyCategory);
            return supplyCategory.Id;
        }
    }

 
    public class UpdateCommand : IRequest<ApiResponse<SupplyCategory>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ApiResponse<SupplyCategory>>
    {
        private readonly ISupplyCategoryRepository _supplyCategoryRepository;

        public UpdateCommandHandler(ISupplyCategoryRepository supplyCategoryRepository)
        {
            _supplyCategoryRepository = supplyCategoryRepository;
        }

        public async Task<ApiResponse<SupplyCategory>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var existing = await _supplyCategoryRepository.GetByIdAsync(request.Id);

            if (existing == null)
                return ApiResponse<SupplyCategory>.Failure("Categoría no encontrada");

            existing.Name = request.Name;
            existing.Description = request.Description;

            var updated = await _supplyCategoryRepository.UpdateAsync(existing);

            return ApiResponse<SupplyCategory>.Success(updated, "Categoría actualizada correctamente");
        }
    }
   
    public class DeleteCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
    }

    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ApiResponse<string>>
    {
        private readonly ISupplyCategoryRepository _supplyCategoryRepository;

        public DeleteCommandHandler(ISupplyCategoryRepository supplyCategoryRepository)
        {
            _supplyCategoryRepository = supplyCategoryRepository;
        }

        public async Task<ApiResponse<string>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var existing = await _supplyCategoryRepository.GetByIdAsync(request.Id);

            if (existing == null)
                return ApiResponse<string>.Failure("Categoría no encontrada");

            await _supplyCategoryRepository.DeleteAsync(existing);

            return ApiResponse<string>.Success("Categoría eliminada correctamente");
        }
    }
}