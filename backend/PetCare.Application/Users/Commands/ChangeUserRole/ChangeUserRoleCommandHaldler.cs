using MediatR;
using Microsoft.AspNetCore.Identity;
using PetCare.Application.Common;
using PetCare.Domain.Identity;

namespace PetCare.Application.Users.Commands.ChangeUserRole
{
    public class ChangeUserRoleCommandHandler :
        IRequestHandler<ChangeUserRoleCommand, ApiResponse<string>>
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ChangeUserRoleCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<string>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken) 
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if(user == null)
                return ApiResponse<string>.Failure("Usuario no encontrado");

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if(!removeResult.Succeeded)
                return ApiResponse<string>.Failure("Error al eliminar roles del usuario");

            var addResult = await _userManager.AddToRoleAsync(user, request.RoleName);
            if(!addResult.Succeeded)
                return ApiResponse<string>.Failure("Error al asignar el nuevo rol al usuario");

            return ApiResponse<string>.Success("Rol cambiado exitosamente");
        }
    }
    
}

