using PetCare.Domain.Enums;
using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetByIdAsync(Guid id);
        Task<Invoice?> GetByIdWithItemsAsync(Guid id);
        Task<List<Invoice>> GetByOwnerIdAsync(int ownerId);
        Task<Invoice?> GetByAppointmentIdAsync(int appointmentId);
        Task<List<Invoice>> GetAllAsync(DateTime? from, DateTime? to, InvoiceStatus? status);
        Task<string> GetLastInvoiceNumberAsync();
        Task AddAsync(Invoice invoice);
        Task UpdateAsync(Invoice invoice);
    }
}