using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PetCare.Application.Users.Commands.DeleteUser;
using PetCare.Domain.Identity;
using PetCare.Tests.TestHelpers;

namespace PetCare.Tests.Users
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerTests()
        {
            _userManager = MockUserManager.Create();
            _handler = new DeleteUserCommandHandler(_userManager.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsFailure()
        {
            _userManager
                .Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _handler.Handle(new DeleteUserCommand { Id = "1" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _userManager.Verify(m => m.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeleteAsyncFails_ReturnsFailure()
        {
            var user = new ApplicationUser { Id = "1" };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed());

            var result = await _handler.Handle(new DeleteUserCommand { Id = "1" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Error al eliminar el usuario", result.Message);
        }

        [Fact]
        public async Task Handle_DeleteThrowsDbUpdateException_ReturnsFriendlyFailureMessage()
        {
            // Simula que el usuario tiene historiales médicos asociados como
            // veterinario y la base de datos rechaza el borrado por la
            // restricción de clave foránea.
            var user = new ApplicationUser { Id = "1" };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.DeleteAsync(user)).ThrowsAsync(new DbUpdateException());

            var result = await _handler.Handle(new DeleteUserCommand { Id = "1" }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains("historiales médicos", result.Message);
        }

        [Fact]
        public async Task Handle_ValidUser_DeletesSuccessfully()
        {
            var user = new ApplicationUser { Id = "1" };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(new DeleteUserCommand { Id = "1" }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _userManager.Verify(m => m.DeleteAsync(user), Times.Once);
        }
    }
}
