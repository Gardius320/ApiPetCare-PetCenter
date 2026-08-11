using Moq;
using PetCare.Application.Auth.Commands.Logout;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Auth
{
    public class LogoutCommandHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly LogoutCommandHandler _handler;

        public LogoutCommandHandlerTests()
        {
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _handler = new LogoutCommandHandler(_refreshTokenRepository.Object);
        }

        [Fact]
        public async Task Handle_TokenNotFound_DoesNotRevokeAnything()
        {
            _refreshTokenRepository.Setup(r => r.GetByTokenAsync(It.IsAny<string>())).ReturnsAsync((RefreshToken?)null);

            var command = new LogoutCommand { RefreshToken = "unknown-token", UserId = "1" };

            await _handler.Handle(command, CancellationToken.None);

            _refreshTokenRepository.Verify(r => r.RevokeAsync(It.IsAny<RefreshToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_TokenBelongsToDifferentUser_DoesNotRevoke()
        {
            var stored = new RefreshToken { Token = "some-token", UserId = "owner-1", IsRevoked = false };
            _refreshTokenRepository.Setup(r => r.GetByTokenAsync("some-token")).ReturnsAsync(stored);

            var command = new LogoutCommand { RefreshToken = "some-token", UserId = "attacker-2" };

            await _handler.Handle(command, CancellationToken.None);

            _refreshTokenRepository.Verify(r => r.RevokeAsync(It.IsAny<RefreshToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_TokenAlreadyRevoked_DoesNotRevokeAgain()
        {
            var stored = new RefreshToken { Token = "some-token", UserId = "1", IsRevoked = true };
            _refreshTokenRepository.Setup(r => r.GetByTokenAsync("some-token")).ReturnsAsync(stored);

            var command = new LogoutCommand { RefreshToken = "some-token", UserId = "1" };

            await _handler.Handle(command, CancellationToken.None);

            _refreshTokenRepository.Verify(r => r.RevokeAsync(It.IsAny<RefreshToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidOwnedToken_RevokesToken()
        {
            var stored = new RefreshToken { Token = "some-token", UserId = "1", IsRevoked = false };
            _refreshTokenRepository.Setup(r => r.GetByTokenAsync("some-token")).ReturnsAsync(stored);
            _refreshTokenRepository.Setup(r => r.RevokeAsync(stored)).Returns(Task.CompletedTask);

            var command = new LogoutCommand { RefreshToken = "some-token", UserId = "1" };

            await _handler.Handle(command, CancellationToken.None);

            _refreshTokenRepository.Verify(r => r.RevokeAsync(stored), Times.Once);
        }
    }
}
