using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<ApiResponse<List<UserDto>>>
    {
    }
}