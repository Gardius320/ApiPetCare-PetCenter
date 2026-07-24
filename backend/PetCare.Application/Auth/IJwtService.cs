using PetCare.Domain.Identity;

namespace PetCare.Application.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles);
        string GenerateRefreshToken(); 

        TimeSpan GetRefreshTokenDuration(IList<string> roles);
        int GetAccessTokenDurationMinutes();
    }
}
