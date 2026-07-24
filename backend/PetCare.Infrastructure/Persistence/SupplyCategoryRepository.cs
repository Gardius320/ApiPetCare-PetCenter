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

        public async Task<SupplyCategory> Create(SupplyCategory category)
        {
            category.IsActive = true;
            await _context.SupplyCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
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

        public async Task<SupplyCategory> UpdateAsync(SupplyCategory category)
        {
            _context.SupplyCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        } 
        
        public async Task<SupplyCategory> DeleteAsync(SupplyCategory category)
        {
            _context.SupplyCategories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }



    }
}