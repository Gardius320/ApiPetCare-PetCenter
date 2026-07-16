using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner?> CreateOwner(Owner owner);
        Task<(List<Owner> owners, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null);
        Task<Owner?> GetByIdAsync(int id);
        Task<Owner?> UpdateOwner(Owner owner);
        Task<string> DeleteOwner(int id);
    }
}
