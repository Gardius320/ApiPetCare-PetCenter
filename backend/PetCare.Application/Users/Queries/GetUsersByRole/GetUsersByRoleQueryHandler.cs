using MediatR;
using Microsoft.AspNetCore.Identity;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;
using PetCare.Domain.Identity;

namespace PetCare.Application.Users.Queries.GetUsersByRole
{
    public class GetUsersByRoleQueryHandler
        : IRequestHandler<GetUsersByRoleQuery, ApiResponse<List<UserSummaryDto>>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUsersByRoleQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<List<UserSummaryDto>>> Handle(
            GetUsersByRoleQuery request,
            CancellationToken cancellationToken)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Role);

            var result = usersInRole.Select(u => new UserSummaryDto
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}"
            }).ToList();

            return ApiResponse<List<UserSummaryDto>>.Success(result);
        }
    }
}
