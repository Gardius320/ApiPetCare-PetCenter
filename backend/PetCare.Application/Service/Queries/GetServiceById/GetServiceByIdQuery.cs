using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Services.Queries.GetServiceById
{
    public class GetServiceByIdQuery : IRequest<ApiResponse<ServiceDTO>>
    {
        public int Id { get; set; }
    }
}