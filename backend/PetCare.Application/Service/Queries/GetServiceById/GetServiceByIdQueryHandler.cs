using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Services.Queries.GetServiceById
{
    public class GetServiceByIdQueryHandler
        : IRequestHandler<GetServiceByIdQuery, ApiResponse<ServiceDTO>>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServiceByIdQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<ApiResponse<ServiceDTO>> Handle(
            GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByIdAsync(request.Id);

            if (service == null)
            {
                return ApiResponse<ServiceDTO>.Failure("Servicio no encontrado.");
            }

            var dto = new ServiceDTO
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                IsActive = service.IsActive
            };

            return ApiResponse<ServiceDTO>.Success(dto);
        }
    }
}