using Microsoft.AspNetCore.Identity;
using Moq;
using PetCare.Domain.Identity;

namespace PetCare.Tests.TestHelpers
{
    public static class MockUserManager
    {
        public static Mock<UserManager<ApplicationUser>> Create()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }
    }
}
