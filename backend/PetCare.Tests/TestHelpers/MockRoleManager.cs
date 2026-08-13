using Microsoft.AspNetCore.Identity;
using Moq;

namespace PetCare.Tests.TestHelpers
{
    // Mismo motivo que MockUserManager: RoleManager<TRole> no es una interfaz,
    // así que para mockearlo hay que pasarle un IRoleStore falso y dejar el
    // resto de dependencias del constructor en null (no se usan en los tests).
    public static class MockRoleManager
    {
        public static Mock<RoleManager<IdentityRole>> Create()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(
                store.Object, null!, null!, null!, null!);
        }
    }
}
