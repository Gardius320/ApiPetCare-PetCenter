using MediatR;
using Microsoft.AspNetCore.Identity;
using PetCare.Domain.Identity;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token inválido o expirado");

            var user = storedToken.User;
            var roles = await _userManager.GetRolesAsync(user);

            await _refreshTokenRepository.RevokeAsync(storedToken);

            var newAccessToken = _jwtService.GenerateAccessToken(user, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // 👇 Antes: DateTime.UtcNow.AddDays(7) fijo — aquí nace el efecto "deslizante"
            var sessionDuration = _jwtService.GetRefreshTokenDuration(roles);

            await _refreshTokenRepository.AddAsync(new PetCare.Domain.Models.RefreshToken
            {
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.Add(sessionDuration),
                IsRevoked = false,
                UserId = user.Id
            });

            return new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Email = user.Email!,
                FullName = user.FullName,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };
        }
    }
}