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
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Usuario no encontrado");

            bool correctPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!correctPassword)
                throw new UnauthorizedAccessException("Contraseña incorrecta");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            
            var sessionDuration = _jwtService.GetRefreshTokenDuration(roles);

            await _refreshTokenRepository.AddAsync(new PetCare.Domain.Models.RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.Add(sessionDuration),
                IsRevoked = false,
                UserId = user.Id
            });

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Email = user.Email!,
                FullName = user.FullName,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Expires = DateTime.UtcNow.AddMinutes(_jwtService.GetAccessTokenDurationMinutes())
            };
        }
    }
}