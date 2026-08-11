using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Users.Queries.GetUsersByRole
{
    public class GetUsersByRoleQuery : IRequest<ApiResponse<List<UserSummaryDto>>>
    {
        public string Role { get; set; } = string.Empty;
    }
}
