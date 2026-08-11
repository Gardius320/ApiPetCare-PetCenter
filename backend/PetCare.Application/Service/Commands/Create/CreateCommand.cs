using FluentValidation;
using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Services.Commands.Create
{
    public class CreateCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(150).WithMessage("La descripción no puede exceder los 150 caracteres.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("El precio debe ser mayor o igual a cero.");
        }
    }

    public class CreateCommandHandler : IRequestHandler<CreateCommand, int>
    {
        private readonly IServiceRepository _serviceRepository;

        public CreateCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<int> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var service = new Service
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            await _serviceRepository.CreateService(service);
            return service.Id;
        }
    }

    public class UpdateCommand : IRequest<ApiResponse<Service>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(150).WithMessage("La descripción no puede exceder los 150 caracteres.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("El precio debe ser mayor o igual a cero.");
        }
    }

    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ApiResponse<Service>>
    {
        private readonly IServiceRepository _serviceRepository;

        public UpdateCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<ApiResponse<Service>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByIdAsync(request.Id);

            if (service == null)
            {
                return ApiResponse<Service>.Failure("Servicio no encontrado.");
            }

            service.Name = request.Name;
            service.Description = request.Description;
            service.Price = request.Price;

            var updatedService = await _serviceRepository.UpdateService(service);

            return ApiResponse<Service>.Success(updatedService, "Servicio actualizado correctamente.");
        }
    }

    public class DeleteCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
    }

    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ApiResponse<string>>
    {
        private readonly IServiceRepository _serviceRepository;

        public DeleteCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<ApiResponse<string>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var message = await _serviceRepository.DeleteService(request.Id);
            return ApiResponse<string>.Success(message);
        }
    }    
}