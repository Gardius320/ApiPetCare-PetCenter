using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly PetsDbContext _context;

        public OwnerRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<Owner?> CreateOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return owner;
        }

        public async Task<(List<Owner> owners, int totalRecords)> GetAllPagesAsync(
            int page, int pageSize, string? search = null)
        {
            var query = _context.Owners.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(o =>
                    o.OwnerName.Contains(search) ||
                    o.Email.Contains(search)
                );
            }

            int totalRecords = await query.CountAsync();

            var owners = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (owners, totalRecords);
        }

        public async Task<Owner?> GetByIdAsync(int id)
        {
            return await _context.Owners.FindAsync(id);
        }

        public async Task<Owner?> UpdateOwner(Owner owner)
        {
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return owner;
        }

        public async Task<string> DeleteOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);

            if (owner == null) return "Propietario no encontrado";

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            return "Propietario eliminado exitosamente";
        }
        public async Task<(int petsCount, int appointmentsCount)> GetDependencyCountsAsync(int ownerId)
        {
            int petsCount = await _context.Pets.CountAsync(p => p.OwnerId == ownerId);
            int appointmentsCount = await _context.Appointments.CountAsync(a => a.OwnerId == ownerId);
            return (petsCount, appointmentsCount);
        }
    }
}
