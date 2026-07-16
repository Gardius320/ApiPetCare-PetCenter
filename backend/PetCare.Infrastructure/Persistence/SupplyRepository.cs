using Microsoft.EntityFrameworkCore;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class SupplyRepository : ISupplyRepository
    {
        private readonly PetsDbContext _context;

        public SupplyRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<Supply?> CreateSupply(Supply supply)
        {
            _context.Supplies.Add(supply);
            await _context.SaveChangesAsync();
            return supply;
        }

        public async Task<string> DeleteSupply(int id)
        {
            var product = await _context.Supplies.FindAsync(id);
            if(product == null) return "Producto no encontrado";
            _context.Supplies.Remove(product);
            await _context.SaveChangesAsync();
            return "Producto eliminado correctamente";
        }

        public async Task<Supply?> GetByIdAsync(int id)
        {
            return await _context.Supplies
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Supply?> UpdateSupply(Supply supply) 
        {
            _context.Supplies.Update(supply);
            await _context.SaveChangesAsync();
            return supply;
        }

        public async Task<(List<Supply> supplies, int totalRecords)> GetAllPagesAsync(
         int page, int pageSize, string? search = null, int? categoryId = null, bool onlyActive = true)
        {
            var query = _context.Supplies
                .Include(s => s.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Name.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(s => s.SupplyCategoryId == categoryId.Value);
            }

            if (onlyActive)
            {
                query = query.Where(s => s.IsActive);
            }

            int totalRecords = await query.CountAsync();

            var supplies = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (supplies, totalRecords);
        }
        public async Task<SupplyStatsDTO> GetStatsAsync()
        {
            var activeQuery = _context.Supplies.Where(s => s.IsActive);

            var total = await activeQuery.CountAsync();

            var agotados = await activeQuery
                .CountAsync(s => s.CurrentStock == 0);

            var bajoStock = await activeQuery
                .CountAsync(s => s.CurrentStock > 0 && s.CurrentStock <= s.MinimumStock);

            var categoriasEnUso = await activeQuery
                .Select(s => s.SupplyCategoryId)
                .Distinct()
                .CountAsync();

            return new SupplyStatsDTO
            {
                Total = total,
                BajoStock = bajoStock,
                Agotados = agotados,
                CategoriasEnUso = categoriasEnUso
            };
        }
    }
}
