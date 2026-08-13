using Microsoft.AspNetCore.Identity;
using Moq;
using PetCare.Application.Users.Commands.CreateUsers;
using PetCare.Domain.Identity;
using PetCare.Tests.TestHelpers;

namespace PetCare.Tests.Users
{
    // Este módulo no usa un repositorio propio: usa UserManager<ApplicationUser>
    // de ASP.NET Identity. Se mockea igual que en Auth (ver TestHelpers/MockUserManager),
    // porque UserManager no es una interfaz sino una clase con métodos virtuales.
    public class CreateUsersCommandHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly CreateUsersCommandHandler _handler;

        public CreateUsersCommandHandlerTests()
        {
            _userManager = MockUserManager.Create();
            _handler = new CreateUsersCommandHandler(_userManager.Object);
        }

        [Fact]
        public async Task Handle_EmailAlreadyExists_ReturnsFailure()
        {
            _userManager
                .Setup(m => m.FindByEmailAsync("existe@petcare.com"))
                .ReturnsAsync(new ApplicationUser { Id = "1", Email = "existe@petcare.com" });

            var command = new CreateUsersCommand
            {
                Email = "existe@petcare.com",
                Password = "Password123!",
                FirstName = "Juan",
                LastName = "Pérez",
                Role = "Vet"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _userManager.Verify(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_CreateAsyncFails_ReturnsFailureWithErrorDescription()
        {
            _userManager
                .Setup(m => m.FindByEmailAsync("nuevo@petcare.com"))
                .ReturnsAsync((ApplicationUser?)null);
            _userManager
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "La contraseña es muy débil" }));

            var command = new CreateUsersCommand
            {
                Email = "nuevo@petcare.com",
                Password = "123",
                FirstName = "Juan",
                LastName = "Pérez",
                Role = "Vet"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains("La contraseña es muy débil", result.Message);
            _userManager.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidUser_CreatesAndAssignsRole()
        {
            _userManager
                .Setup(m => m.FindByEmailAsync("nuevo@petcare.com"))
                .ReturnsAsync((ApplicationUser?)null);
            _userManager
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), "Password123!"))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Vet"))
                .ReturnsAsync(IdentityResult.Success);

            var command = new CreateUsersCommand
            {
                Email = "nuevo@petcare.com",
                Password = "Password123!",
                FirstName = "Juan",
                LastName = "Pérez",
                Role = "Vet"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _userManager.Verify(m => m.AddToRoleAsync(It.Is<ApplicationUser>(u =>
                u.Email == "nuevo@petcare.com" &&
                u.FirstName == "Juan" &&
                u.LastName == "Pérez"), "Vet"), Times.Once);
        }
    }
}
