using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class StateRepository : IStateRepository
    {
        private readonly PetsDbContext _context;

        public StateRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<List<State>> GetAll()
        {
            return await _context.States
                .OrderBy(s => s.StateName)
                .ToListAsync();
        }

        public async Task<State?> GetById(int id)
        {
            return await _context.States.FindAsync(id);
        }

        public async Task<State> CreateState(State state)
        {
            var newState = new State
            {
                StateName   = state.StateName,
                Description = state.Description
            };

            _context.States.Add(newState);
            await _context.SaveChangesAsync();
            return newState;
        }

        public async Task<State> UpdateAsync(State state)
        {
            _context.States.Update(state);
            await _context.SaveChangesAsync();
            return state;
        }

        public async Task<string> DeleteState(int id)
        {
            var state = await _context.States.FindAsync(id);

            if (state == null) return "Estado no encontrado";

            try
            {
                _context.States.Remove(state);
                await _context.SaveChangesAsync();
                return "Estado eliminado con éxito";
            }
            catch (Exception)
            {
                return "No se puede eliminar el estado porque tiene citas asociadas";
            }
        }
    }
}
