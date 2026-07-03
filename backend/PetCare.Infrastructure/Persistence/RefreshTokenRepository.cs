using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly PetsDbContext _context;

        public RefreshTokenRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
            => await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAsync(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }
}
