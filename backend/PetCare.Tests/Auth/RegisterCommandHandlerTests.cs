using Microsoft.AspNetCore.Identity;
using Moq;
using PetCare.Application.Auth.Commands.Register;
using PetCare.Domain.Identity;
using PetCare.Tests.TestHelpers;

namespace PetCare.Tests.Auth
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<RoleManager<IdentityRole>> _roleManager;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _userManager = MockUserManager.Create();
            _roleManager = MockRoleManager.Create();
            _handler = new RegisterCommandHandler(_userManager.Object, _roleManager.Object);
        }

        [Fact]
        public async Task Handle_EmailAlreadyExists_ThrowsInvalidOperationException()
        {
            _userManager
                .Setup(m => m.FindByEmailAsync("existe@petcare.com"))
                .ReturnsAsync(new ApplicationUser { Id = "1", Email = "existe@petcare.com" });

            var command = new RegisterCommand
            {
                Email = "existe@petcare.com",
                Password = "Password123!",
                FirstName = "Juan",
                LastName = "Pérez",
                Role = "Vet"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            _userManager.Verify(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_CreateAsyncFails_ThrowsInvalidOperationExceptionWithErrors()
        {
            _userManager
                .Setup(m => m.FindByEmailAsync("nuevo@petcare.com"))
                .ReturnsAsync((ApplicationUser?)null);
            _userManager
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "La contraseña es muy débil" }));

            var command = new RegisterCommand
            {
                Email = "nuevo@petcare.com",
                Password = "123",
                FirstName = "Juan",
                LastName = "Pérez",
                Role = "Vet"
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("La contraseña es muy débil", ex.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_CreatesUserAssignsRoleAndReturnsMessage()
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

            var command = new RegisterCommand
            {
                Email = "nuevo@petcare.com",
                Password = "Password123!",
                FirstName = "Juan",
                LastName = "Pérez",
                Role = "Vet"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Usuario registrado correctamente", result);
            _userManager.Verify(m => m.AddToRoleAsync(It.Is<ApplicationUser>(u =>
                u.Email == "nuevo@petcare.com"), "Vet"), Times.Once);
        }
    }
}
