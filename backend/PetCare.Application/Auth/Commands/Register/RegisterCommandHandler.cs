using MediatR;
using Microsoft.AspNetCore.Identity;
using PetCare.Domain.Identity;

namespace PetCare.Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var usuarioExiste = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioExiste != null)
                throw new InvalidOperationException("El usuario ya existe");

            var nuevoUsuario = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var resultado = await _userManager.CreateAsync(nuevoUsuario, request.Password);
            if (!resultado.Succeeded)
            {
                var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errores);
            }

            await _userManager.AddToRoleAsync(nuevoUsuario, request.Role);

            return "Usuario registrado correctamente";
        }
    }
}