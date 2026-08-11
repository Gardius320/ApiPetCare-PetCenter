using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IMedicalRecordRepository
    {
        Task<MedicalRecord?> CreateMedicalRecordAsync(MedicalRecord medicalRecord);
        Task<MedicalRecord?> GetByIdAsync(int id);
        Task<List<MedicalRecord>> GetAllAsync(DateTime? from, DateTime? to, string? vetId);
        Task<List<MedicalRecord>> GetByPetIdAsync(int petId);
        Task<MedicalRecord?> UpdateMedicalRecordAsync(MedicalRecord record);
        Task<string> DeleteMedicalRecordAsync(int id);
        Task<List<int>> GetUsedAppointmentIdsAsync(int petId);
    }
}