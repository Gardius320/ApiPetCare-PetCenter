// Repositorio que accede a la tabla de especies en la base de datos
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly PetsDbContext _context;

        public SpeciesRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Species>> GetAll()
        {
            return await _context.Species.OrderBy(s => s.SpecieName).ToListAsync();
        }

        public async Task<Species?> CreateSpecies(Species species)
        {
            _context.Species.Add(species);
            await _context.SaveChangesAsync();
            return species;
        }

        public async Task<string> DeleteSpecies(int id)
        {
            var specie = await _context.Species.FindAsync(id);

            if (specie == null) return "Especie no encontrada";

            try
            {
                _context.Species.Remove(specie);
                await _context.SaveChangesAsync();
                return "Especie eliminada exitosamente";
            }
            catch (Exception)
            {
                return "Error al eliminar la especie";
            }
        }
    }
}