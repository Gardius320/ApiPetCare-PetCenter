using MediatR;
using PetCare.Application.Common;

namespace PetCare.Application.Users.Commands.ChangeUserRole
{
    public  class ChangeUserRoleCommand : IRequest<ApiResponse<string>>
    {
        public string? UserId { get; set; }
        public string? RoleName { get; set; }
    }
}
