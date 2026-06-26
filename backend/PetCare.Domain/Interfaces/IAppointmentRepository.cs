using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> CreateAppointment(int ownerId, DateTime appointmentDate, string description, int? petId = null);

        Task<(List<Appointment> appointments, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null);

       
        Task<bool> CancelAppointment(int id);

        
        Task<bool> UpdateAppointment(int id, DateTime newDate, string newObservation);
    }
}
