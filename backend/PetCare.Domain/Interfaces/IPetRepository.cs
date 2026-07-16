using PetCare.Domain.DTOs;
using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IPetRepository
    {
       
        Task<(List<Pet> pets, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null);

        Task<Pet?> GetByIdAsync(int id);

        Task<Pet?> CreatePetAsync(Pet pet);

        Task<Pet?> UpdatePet(int id, Pet pet);

        Task<string> PetDelete(int id);

        Task<string> ChangePetState(int id, bool isActive);

        Task<PetStatsDTO> GetPetStatsAsync();
    }
}
