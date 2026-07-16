using MediatR;
using Microsoft.AspNetCore.Identity;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;
using PetCare.Domain.Identity;

namespace PetCare.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, ApiResponse<List<UserDto>>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<List<UserDto>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Role = roles.FirstOrDefault() ?? "Sin rol"
                });
            }

            return ApiResponse<List<UserDto>>.Success(result);
        }
    }
}