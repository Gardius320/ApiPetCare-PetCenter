using MediatR;
using Microsoft.AspNetCore.Identity;
using PetCare.Application.Common;
using PetCare.Domain.Identity;

namespace PetCare.Application.Users.Commands.CreateUsers
{
    public class CreateUsersCommandHandler
        : IRequestHandler<CreateUsersCommand, ApiResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUsersCommandHandler(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<string>> Handle(CreateUsersCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return ApiResponse<string>.Failure("El usuario ya existe");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return ApiResponse<string>.Failure("Error al crear el usuario");

            return ApiResponse<string>.Success("Usuario creado exitosamente");
        }
    }
}
