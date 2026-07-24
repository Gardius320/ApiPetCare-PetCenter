using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface ISupplyCategoryRepository
    {
        Task<List<SupplyCategory>> GetAllAsync();
        Task<SupplyCategory?> GetByIdAsync(int id);

        Task<SupplyCategory> Create(SupplyCategory category);
        
        Task<SupplyCategory> UpdateAsync(SupplyCategory category);

        Task<SupplyCategory> DeleteAsync(SupplyCategory category);
    }
}