using PetCare.Domain.Models;

namespace PetCare.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> CreateAppointment(int ownerId, DateTime appointmentDate, string description, int? petId = null, string? stateName = null);

        Task<Appointment?> GetByIdAsync(int id);

        Task<(List<Appointment> appointments, int totalRecords)> GetAllPagesAsync(int page, int pageSize, string? search = null);
        Task<bool> ChangeAppointmentState(int id, int stateId);

        Task<bool> CancelAppointment(int id);
        
        Task<bool> UpdateAppointment(int id, DateTime newDate, string newObservation);

        Task<List<Appointment>> GetByPetIdAsync(int petId);

        Task<List<Appointment>> GetBillableAppointmentsAsync(int ownerId);

        Task<List<Appointment>> GetScheduledAppointmentsForDateAsync(DateTime date);
    }
}
