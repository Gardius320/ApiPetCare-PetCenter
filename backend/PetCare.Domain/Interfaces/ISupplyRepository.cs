using PetCare.Domain.Models;
using PetCare.Domain.DTOs;

namespace PetCare.Domain.Interfaces
{
    public interface ISupplyRepository
    {
        Task<Supply?> CreateSupply(Supply supply);
        Task<(List<Supply> supplies, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null, int? categoryId = null, bool onlyActive = true);
        Task<Supply?> GetByIdAsync(int id);
        Task<Supply?> UpdateSupply(Supply supply);
        Task<string> DeleteSupply(int id);
        Task<SupplyStatsDTO> GetStatsAsync();
    }
}