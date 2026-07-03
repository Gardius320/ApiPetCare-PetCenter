using MediatR;
using Microsoft.AspNetCore.Identity;
using PetCare.Domain.Identity;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
                throw new UnauthorizedAccessException("Email o contraseña incorrectos");

            bool contraseñaCorrecta = await _userManager.CheckPasswordAsync(usuario, request.Password);
            if (!contraseñaCorrecta)
                throw new UnauthorizedAccessException("Email o contraseña incorrectos");

            var roles = await _userManager.GetRolesAsync(usuario);
            var accessToken = _jwtService.GenerateAccessToken(usuario, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            
            var duracionSesion = _jwtService.GetRefreshTokenDuration(roles);

            await _refreshTokenRepository.AddAsync(new PetCare.Domain.Models.RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.Add(duracionSesion),
                IsRevoked = false,
                UserId = usuario.Id
            });

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Email = usuario.Email!,
                FullName = usuario.FullName,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };
        }
    }
}