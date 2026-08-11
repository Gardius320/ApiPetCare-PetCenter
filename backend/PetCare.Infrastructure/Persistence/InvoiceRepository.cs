using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly PetsDbContext _context;

        public InvoiceRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice?> GetByIdAsync(Guid id)
        {
            return await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice?> GetByIdWithItemsAsync(Guid id)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Invoice>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Invoices
                .Where(i => i.OwnerId == ownerId)
                .OrderByDescending(i => i.IssueDate)
                .ToListAsync();
        }

        public async Task<Invoice?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Invoices
                .FirstOrDefaultAsync(i => i.AppointmentId == appointmentId);
        }

        public async Task<List<Invoice>> GetAllAsync(DateTime? from, DateTime? to, InvoiceStatus? status)
        {
            var query = _context.Invoices.AsQueryable();

            if (from.HasValue)
                query = query.Where(i => i.IssueDate >= from.Value);

            if (to.HasValue)
                query = query.Where(i => i.IssueDate <= to.Value);

            if (status.HasValue)
                query = query.Where(i => i.Status == status.Value);

            return await query
                .OrderByDescending(i => i.IssueDate)
                .ToListAsync();
        }

        public async Task<string> GetLastInvoiceNumberAsync()
        {
            var last = await _context.Invoices
                .OrderByDescending(i => i.IssueDate)
                .Select(i => i.InvoiceNumber)
                .FirstOrDefaultAsync();

            return last ?? "FAC-0000";
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
    }
}