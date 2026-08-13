using Microsoft.AspNetCore.Identity;
using Moq;
using PetCare.Application.Users.Commands.ChangeUserRole;
using PetCare.Domain.Identity;
using PetCare.Tests.TestHelpers;

namespace PetCare.Tests.Users
{
    public class ChangeUserRoleCommandHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly ChangeUserRoleCommandHandler _handler;

        public ChangeUserRoleCommandHandlerTests()
        {
            _userManager = MockUserManager.Create();
            _handler = new ChangeUserRoleCommandHandler(_userManager.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsFailure()
        {
            _userManager
                .Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser?)null);

            var command = new ChangeUserRoleCommand { UserId = "1", RoleName = "Admin" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _userManager.Verify(m => m.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RemoveRolesFails_ReturnsFailure()
        {
            var user = new ApplicationUser { Id = "1", Email = "vet@petcare.com" };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Vet" });
            _userManager
                .Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed());

            var command = new ChangeUserRoleCommand { UserId = "1", RoleName = "Admin" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Error al eliminar roles del usuario", result.Message);
            _userManager.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_AddRoleFails_ReturnsFailure()
        {
            var user = new ApplicationUser { Id = "1", Email = "vet@petcare.com" };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Vet" });
            _userManager
                .Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(m => m.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Failed());

            var command = new ChangeUserRoleCommand { UserId = "1", RoleName = "Admin" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Error al asignar el nuevo rol al usuario", result.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_ChangesRoleSuccessfully()
        {
            var user = new ApplicationUser { Id = "1", Email = "vet@petcare.com" };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Vet" });
            _userManager
                .Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(m => m.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            var command = new ChangeUserRoleCommand { UserId = "1", RoleName = "Admin" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _userManager.Verify(m => m.AddToRoleAsync(user, "Admin"), Times.Once);
        }
    }
}
