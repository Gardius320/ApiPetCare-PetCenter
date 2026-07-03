using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task RevokeAsync(RefreshToken refreshToken);
    }
}
