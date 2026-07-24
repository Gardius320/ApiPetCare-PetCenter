using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (token == null || token.UserId != request.UserId || token.IsRevoked)
            {
                return;
            }

            await _refreshTokenRepository.RevokeAsync(token);
        }
    }
}
