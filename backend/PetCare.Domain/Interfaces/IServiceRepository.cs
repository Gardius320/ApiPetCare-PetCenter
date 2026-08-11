using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IServiceRepository
    {

        Task<Service?> CreateService(Service service);
        Task<(List<Service> services, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null, bool onlyActive = true);
        Task<Service?> GetByIdAsync(int id);
        Task<Service?> UpdateService(Service service);
        Task<string> DeleteService(int id);
    }
}
