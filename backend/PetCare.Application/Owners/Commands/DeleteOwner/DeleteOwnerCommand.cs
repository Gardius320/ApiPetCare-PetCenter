using MediatR;
using PetCare.Application.Common;

namespace PetCare.Application.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
    }
}