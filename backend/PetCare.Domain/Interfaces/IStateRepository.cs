using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IStateRepository
    {
        Task<List<State>> GetAll();
        Task<State?> GetById(int Id);
        Task<State> UpdateAsync(State state);
        Task<State?> CreateState(State state);
        Task<string> DeleteState(int id);

    }
}
