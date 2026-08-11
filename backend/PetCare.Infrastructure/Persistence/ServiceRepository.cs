using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class ServiceRepository : IServiceRepository
    {

        private readonly PetsDbContext _context;
        public ServiceRepository(PetsDbContext context)
        {
            _context = context;
        }
        public async Task<Service?> CreateService(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }
        public async Task<string> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if(service == null)
            {
                return "Servicio no encontrado.";
            }
            service.IsActive = false;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return "Servicio desactivado correctamente.";

        }
        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }
        public async Task<Service?> UpdateService(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return service;
        }
        public async Task<(List<Service> services, int totalRecords)> GetAllPagesAsync(
         int page, int pageSize, string? search = null, bool onlyActive = true)
        {
            var query = _context.Services.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Name.Contains(search));
            }
            if (onlyActive)
            {
                query = query.Where(s => s.IsActive);
            }
            var totalRecords = await query.CountAsync();
            var services = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (services, totalRecords);
        }
    }
}
