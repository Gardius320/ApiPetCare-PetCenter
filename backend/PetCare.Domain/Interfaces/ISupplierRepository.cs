using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface ISupplierRepository
    {

        Task<Supplier?> CreateSupplier(Supplier supplier);
        Task<(List<Supplier> suppliers, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null, bool onlyActive = true);
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier?> UpdateSupplier(Supplier supplier);
        Task<string> DeleteSupplier(int id);    
    }
}
