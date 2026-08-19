using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class SupplierRepository : ISupplierRepository
    {

        private readonly PetsDbContext _context;

        public SupplierRepository(PetsDbContext context) 
        {
            _context = context;
        }        

        public async Task <Supplier?> CreateSupplier(Supplier supplier)
        {
            supplier.IsActive = true;
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<(List<Supplier> suppliers, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null, bool onlyActive = true)
        {
            var query = _context.Suppliers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Name.Contains(search));
            }
            if (onlyActive)
            {
                query = query.Where(s => s.IsActive);
            }
            int totalRecords = await query.CountAsync();
            var suppliers = await query
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (suppliers, totalRecords);
        }

        public async Task <Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Supplier?> UpdateSupplier(Supplier supplier)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(supplier.Id);
            if (existingSupplier == null)
            {
                return null;
            }
            existingSupplier.Name = supplier.Name;
            existingSupplier.ContactNumber = supplier.ContactNumber;
            existingSupplier.Email = supplier.Email;
            existingSupplier.Address = supplier.Address;
            existingSupplier.IsActive = supplier.IsActive;
            await _context.SaveChangesAsync();
            return existingSupplier;
        }

        public async Task <string> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return "Proveedor no encontrado.";
            }
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return "Proveedor eliminado exitosamente.";
        }
    }
}
