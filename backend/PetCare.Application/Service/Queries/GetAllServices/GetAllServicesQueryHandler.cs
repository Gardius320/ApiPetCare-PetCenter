using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Services.Queries.GetAllServices
{
    public class GetAllServicesQueryHandler
        : IRequestHandler<GetAllServicesQuery, List<ServiceDTO>>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetAllServicesQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<List<ServiceDTO>> Handle(
            GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var (services, totalRecords) = await _serviceRepository.GetAllPagesAsync(
                request.Page, request.PageSize, request.Search, request.OnlyActive);

            var items = new List<ServiceDTO>();
            foreach (var s in services)
            {
                items.Add(new ServiceDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    IsActive = s.IsActive
                });
            }

            return items;
        }
    }
}