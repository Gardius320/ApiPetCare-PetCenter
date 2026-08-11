using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface ISupplyCategoryRepository
    {
        Task<List<SupplyCategory>> GetAllAsync();
        Task<(List<SupplyCategory> categories, int totalRecords)> GetAllPagesAsync(
            int page, int pageSize, string? search = null, bool onlyActive = true);
        Task<SupplyCategory?> GetByIdAsync(int id);

        Task<SupplyCategory> Create(SupplyCategory category);
        
        Task<SupplyCategory> UpdateAsync(SupplyCategory category);

        Task<SupplyCategory> DeleteAsync(SupplyCategory category);
    }
}