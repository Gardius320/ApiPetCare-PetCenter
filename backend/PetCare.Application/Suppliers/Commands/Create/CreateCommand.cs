using FluentValidation;
using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Suppliers.Commands.Create
{
    public class CreateCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
    }

    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");
        }
    }

    public class CreateCommandHandler : IRequestHandler<CreateCommand, int>
    {
        private readonly ISupplierRepository _supplierRepository;
        public CreateCommandHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<int> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var supplier = new Supplier
            {
                Name = request.Name,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                Address = request.Address,
                Description = request.Description
            };
            await _supplierRepository.CreateSupplier(supplier);
            return supplier.Id;
        }
    }

    public class UpdateCommand : IRequest<ApiResponse<Supplier>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");
        }
    }

    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ApiResponse<Supplier>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public UpdateCommandHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<ApiResponse<Supplier>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var existing = await _supplierRepository.GetByIdAsync(request.Id);

            if (existing == null)
                return ApiResponse<Supplier>.Failure("Proveedor no encontrado");

            existing.Name = request.Name;
            existing.ContactNumber = request.ContactNumber;
            existing.Email = request.Email;
            existing.Address = request.Address;
            existing.Description = request.Description;

            var updated = await _supplierRepository.UpdateSupplier(existing);

            return ApiResponse<Supplier>.Success(updated, "Proveedor actualizado correctamente");
        }
    }

    public class DeleteCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
    }

    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ApiResponse<string>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public DeleteCommandHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<ApiResponse<string>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var existing = await _supplierRepository.GetByIdAsync(request.Id);

            if (existing == null)
                return ApiResponse<string>.Failure("Proveedor no encontrado");

            await _supplierRepository.DeleteSupplier(request.Id);

            return ApiResponse<string>.Success("Proveedor eliminado correctamente");
        }
    }
}