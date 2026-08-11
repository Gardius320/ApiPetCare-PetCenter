using MediatR;
using PetCare.Application.Common;

namespace PetCare.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<ApiResponse<string>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
