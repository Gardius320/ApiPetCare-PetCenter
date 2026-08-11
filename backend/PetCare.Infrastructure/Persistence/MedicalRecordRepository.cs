using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly PetsDbContext _context;

        public MedicalRecordRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecord?> CreateMedicalRecordAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Pet)
                .Include(mr => mr.Veterinarian)
                .FirstOrDefaultAsync(mr => mr.Id == id);
        }

        public async Task<List<MedicalRecord>> GetAllAsync(DateTime? from, DateTime? to, string? vetId)
        {
            var query = _context.MedicalRecords
                .Include(mr => mr.Pet)
                .Include(mr => mr.Veterinarian)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(mr => mr.VisitDate >= from.Value);

            if (to.HasValue)
                query = query.Where(mr => mr.VisitDate <= to.Value);

            if (!string.IsNullOrWhiteSpace(vetId))
                query = query.Where(mr => mr.VeterinarianUserId == vetId);

            return await query
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<List<MedicalRecord>> GetByPetIdAsync(int petId)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Pet)
                .Include(mr => mr.Veterinarian)
                .Where(mr => mr.PetId == petId)
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<MedicalRecord?> UpdateMedicalRecordAsync(MedicalRecord record)
        {
            _context.MedicalRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<string> DeleteMedicalRecordAsync(int id)
        {
            var record = await _context.MedicalRecords.FindAsync(id);
            if (record == null)
                return "No se encontró la historia clínica.";

            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
            return "Historia clínica eliminada exitosamente.";
        }
        public async Task<List<int>> GetUsedAppointmentIdsAsync(int petId)
        {
            return await _context.MedicalRecords
                .Where(m => m.PetId == petId && m.AppointmentId != null)
                .Select(m => m.AppointmentId!.Value)
                .ToListAsync();
        }
    }
}