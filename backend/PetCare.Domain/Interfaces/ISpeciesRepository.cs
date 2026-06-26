using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface ISpeciesRepository
    {
        Task<List<Species>> GetAll();
        Task<Species?> CreateSpecies(Species species);
        Task<string> DeleteSpecies(int id);
    }
}