using Microsoft.AspNetCore.Identity;
using Moq;
using PetCare.Application.Auth;
using PetCare.Application.Auth.Commands.Login;
using PetCare.Domain.Identity;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Tests.TestHelpers;

namespace PetCare.Tests.Auth
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IJwtService> _jwtService;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _userManager = MockUserManager.Create();
            _jwtService = new Mock<IJwtService>();
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _handler = new LoginCommandHandler(_userManager.Object, _jwtService.Object, _refreshTokenRepository.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorizedAccessException()
        {
            _userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            var command = new LoginCommand { Email = "missing@petcare.com", Password = "whatever" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WrongPassword_ThrowsUnauthorizedAccessException()
        {
            var user = new ApplicationUser { Id = "1", Email = "admin@petcare.com" };
            _userManager.Setup(m => m.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManager.Setup(m => m.CheckPasswordAsync(user, "wrong-password")).ReturnsAsync(false);

            var command = new LoginCommand { Email = user.Email, Password = "wrong-password" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsTokensAndStoresRefreshToken()
        {
            var user = new ApplicationUser { Id = "1", Email = "admin@petcare.com", FirstName = "Admin", LastName = "PetCare" };
            var roles = new List<string> { "Admin" };

            _userManager.Setup(m => m.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManager.Setup(m => m.CheckPasswordAsync(user, "correct-password")).ReturnsAsync(true);
            _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);

            _jwtService.Setup(j => j.GenerateAccessToken(user, roles)).Returns("access-token");
            _jwtService.Setup(j => j.GenerateRefreshToken()).Returns("refresh-token");
            _jwtService.Setup(j => j.GetRefreshTokenDuration(roles)).Returns(TimeSpan.FromDays(7));
            _jwtService.Setup(j => j.GetAccessTokenDurationMinutes()).Returns(60);

            RefreshToken? stored = null;
            _refreshTokenRepository
                .Setup(r => r.AddAsync(It.IsAny<RefreshToken>()))
                .Callback<RefreshToken>(rt => stored = rt)
                .Returns(Task.CompletedTask);

            var command = new LoginCommand { Email = user.Email, Password = "correct-password" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("access-token", result.Token);
            Assert.Equal("refresh-token", result.RefreshToken);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal("Admin PetCare", result.FullName);
            Assert.Equal("Admin", result.Role);

            Assert.NotNull(stored);
            Assert.Equal("refresh-token", stored!.Token);
            Assert.Equal(user.Id, stored.UserId);
            Assert.False(stored.IsRevoked);
        }

        [Fact]
        public async Task Handle_UserWithNoRoles_ReturnsEmptyRole()
        {
            var user = new ApplicationUser { Id = "1", Email = "noroles@petcare.com" };
            _userManager.Setup(m => m.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManager.Setup(m => m.CheckPasswordAsync(user, "pw")).ReturnsAsync(true);
            _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            _jwtService.Setup(j => j.GenerateAccessToken(user, It.IsAny<IList<string>>())).Returns("access-token");
            _jwtService.Setup(j => j.GenerateRefreshToken()).Returns("refresh-token");
            _jwtService.Setup(j => j.GetRefreshTokenDuration(It.IsAny<IList<string>>())).Returns(TimeSpan.FromDays(1));
            _jwtService.Setup(j => j.GetAccessTokenDurationMinutes()).Returns(60);

            var command = new LoginCommand { Email = user.Email, Password = "pw" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(string.Empty, result.Role);
        }
    }
}
