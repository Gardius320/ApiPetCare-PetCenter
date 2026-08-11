using Microsoft.AspNetCore.Identity;
using Moq;
using PetCare.Application.Auth;
using PetCare.Application.Auth.Commands.RefreshToken;
using PetCare.Domain.Identity;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Tests.TestHelpers;

namespace PetCare.Tests.Auth
{
    public class RefreshTokenCommandHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IJwtService> _jwtService;
        private readonly RefreshTokenCommandHandler _handler;

        public RefreshTokenCommandHandlerTests()
        {
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _userManager = MockUserManager.Create();
            _jwtService = new Mock<IJwtService>();
            _handler = new RefreshTokenCommandHandler(_refreshTokenRepository.Object, _userManager.Object, _jwtService.Object);
        }

        [Fact]
        public async Task Handle_TokenNotFound_ThrowsUnauthorizedAccessException()
        {
            _refreshTokenRepository.Setup(r => r.GetByTokenAsync(It.IsAny<string>())).ReturnsAsync((RefreshToken?)null);

            var command = new RefreshTokenCommand { RefreshToken = "unknown-token" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_TokenRevoked_ThrowsUnauthorizedAccessException()
        {
            var stored = new RefreshToken
            {
                Token = "revoked-token",
                IsRevoked = true,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                User = new ApplicationUser { Id = "1" }
            };
            _refreshTokenRepository.Setup(r => r.GetByTokenAsync("revoked-token")).ReturnsAsync(stored);

            var command = new RefreshTokenCommand { RefreshToken = "revoked-token" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_TokenExpired_ThrowsUnauthorizedAccessException()
        {
            var stored = new RefreshToken
            {
                Token = "expired-token",
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                User = new ApplicationUser { Id = "1" }
            };
            _refreshTokenRepository.Setup(r => r.GetByTokenAsync("expired-token")).ReturnsAsync(stored);

            var command = new RefreshTokenCommand { RefreshToken = "expired-token" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidToken_RevokesOldTokenAndIssuesNewOne()
        {
            var user = new ApplicationUser { Id = "1", Email = "admin@petcare.com", FirstName = "Admin", LastName = "PetCare" };
            var stored = new RefreshToken
            {
                Token = "valid-token",
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                User = user
            };
            var roles = new List<string> { "Admin" };

            _refreshTokenRepository.Setup(r => r.GetByTokenAsync("valid-token")).ReturnsAsync(stored);
            _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);

            RefreshToken? revoked = null;
            _refreshTokenRepository
                .Setup(r => r.RevokeAsync(stored))
                .Callback<RefreshToken>(rt => revoked = rt)
                .Returns(Task.CompletedTask);

            RefreshToken? added = null;
            _refreshTokenRepository
                .Setup(r => r.AddAsync(It.IsAny<RefreshToken>()))
                .Callback<RefreshToken>(rt => added = rt)
                .Returns(Task.CompletedTask);

            _jwtService.Setup(j => j.GenerateAccessToken(user, roles)).Returns("new-access-token");
            _jwtService.Setup(j => j.GenerateRefreshToken()).Returns("new-refresh-token");
            _jwtService.Setup(j => j.GetRefreshTokenDuration(roles)).Returns(TimeSpan.FromDays(7));

            var command = new RefreshTokenCommand { RefreshToken = "valid-token" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Same(stored, revoked);
            Assert.Equal("new-access-token", result.Token);
            Assert.Equal("new-refresh-token", result.RefreshToken);
            Assert.NotNull(added);
            Assert.Equal("new-refresh-token", added!.Token);
            Assert.Equal(user.Id, added.UserId);
        }
    }
}
