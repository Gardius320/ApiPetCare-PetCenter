using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class SupplyCategoryRepository : ISupplyCategoryRepository
    {
        private readonly PetsDbContext _context;

        public SupplyCategoryRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplyCategory>> GetAllAsync()
        {
            return await _context.SupplyCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SupplyCategory?> GetByIdAsync(int id)
        {
            return await _context.SupplyCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}