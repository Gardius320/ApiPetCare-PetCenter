using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Common;
using PetCare.Domain.Identity;

namespace PetCare.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler
        : IRequestHandler<DeleteUserCommand, ApiResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                return ApiResponse<string>.Failure("Usuario no encontrado");

            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return ApiResponse<string>.Failure("Error al eliminar el usuario");
            }
            catch (DbUpdateException)
            {
                return ApiResponse<string>.Failure(
                    "No se puede eliminar el usuario: tiene historiales médicos asociados como veterinario.");
            }

            return ApiResponse<string>.Success("Usuario eliminado correctamente");
        }
    }
}
