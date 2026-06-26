using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Hubs;

namespace PetCare.Infrastructure.Persistence
{
    public class PetsRepository : IPetRepository
    {
        private readonly PetsDbContext _context;
        private readonly IHubContext<PetsHub> _petsHubContext;
        private readonly ILogger<PetsRepository> _logger;

        public PetsRepository(PetsDbContext context, IHubContext<PetsHub> petsHubContext, ILogger<PetsRepository> logger)
        {
            _context       = context;
            _petsHubContext = petsHubContext;
            _logger        = logger;
        }

        public async Task<(List<Pet> pets, int totalRecords)> GetAllPagesAsync(
            int page, int pageSize, string? search = null)
        {
            var query = _context.Pets
                .Include(p => p.Specie)
                .Include(p => p.Owner)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.PetName.Contains(search));
            }

            int totalRecords = await query.CountAsync();

            var pets = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pets, totalRecords);
        }

        public async Task<Pet?> GetByIdAsync(int id)
        {
            return await _context.Pets
                .Include(p => p.Specie)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pet?> CreatePetAsync(Pet pet)
        {
            bool ownerExists = await _context.Owners.AnyAsync(o => o.Id == pet.OwnerId);
            if (!ownerExists)
            {
                _logger.LogWarning("Dueño no encontrado al crear mascota. OwnerId={OwnerId}", pet.OwnerId);
                return null;
            }

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            var createdPet = await _context.Pets
                .Include(p => p.Specie)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == pet.Id);

            await _petsHubContext.Clients.All.SendAsync("PetCreated", createdPet);

            return createdPet;
        }

        public async Task<Pet?> UpdatePet(int id, Pet pet)
        {
            var existing = await _context.Pets.FindAsync(id);

            if (existing == null) return null;

            existing.PetName  = pet.PetName;
            existing.SpecieId = pet.SpecieId;
            existing.OwnerId  = pet.OwnerId;
            existing.IsActive = pet.IsActive;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<string> PetDelete(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null) return "Mascota no encontrada";

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return "Mascota eliminada";
        }

        public async Task<string> ChangePetState(int id, bool isActive)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null) return "Mascota no encontrada";

            pet.IsActive = isActive;
            await _context.SaveChangesAsync();
            return "Estado actualizado";
        }

        public async Task<PetStatsDTO> GetPetStatsAsync()
        {
            var pets = await _context.Pets
                .Include(p => p.Specie)
                .ToListAsync();

            int perrosActivos   = 0;
            int perrosInactivos = 0;
            int gatosActivos    = 0;
            int gatosInactivos  = 0;
            int otrosActivos    = 0;
            int otrosInactivos  = 0;

            foreach (var pet in pets)
            {
                string especie = pet.Specie?.SpecieName ?? string.Empty;

                if (especie == "Perro")
                {
                    if (pet.IsActive) perrosActivos++;
                    else perrosInactivos++;
                }
                else if (especie == "Gato")
                {
                    if (pet.IsActive) gatosActivos++;
                    else gatosInactivos++;
                }
                else
                {
                    if (pet.IsActive) otrosActivos++;
                    else otrosInactivos++;
                }
            }

            return new PetStatsDTO
            {
                Perros = new SpecieStatsDTO { Activos = perrosActivos,  Inactivos = perrosInactivos },
                Gatos  = new SpecieStatsDTO { Activos = gatosActivos,   Inactivos = gatosInactivos  },
                Otros  = new SpecieStatsDTO { Activos = otrosActivos,   Inactivos = otrosInactivos  }
            };
        }
    }
}
