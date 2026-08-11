using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Constants;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PetsDbContext _context;

        public AppointmentRepository(PetsDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> CreateAppointment(int ownerId, DateTime appointmentDate, string description, int? petId = null, string? stateName = null)
        {
            bool ownerExists = await _context.Owners.AnyAsync(o => o.Id == ownerId);
            if (!ownerExists) return null;

            var resolvedStateName = stateName ?? AppointmentStateNames.Scheduled;
            var initialState = await _context.States.FirstOrDefaultAsync(s => s.StateName == resolvedStateName);
            if (initialState == null)
                throw new InvalidOperationException($"El estado '{resolvedStateName}' no existe en la base de datos. Verifica el seed de States.");

            var appointment = new Appointment
            {
                OwnerId = ownerId,
                AppointmentDate = appointmentDate,
                Observation = description,
                StateId = initialState.IdState,
                PetId = petId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.State)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(List<Appointment> appointments, int totalRecords)> GetAllPagesAsync(
            int page, int pageSize, string? search = null)
        {
            var query = _context.Appointments
                .Include(a => a.Owner)
                .Include(a => a.State)
                .Include(a => a.Pet)
                    .ThenInclude(p => p!.Specie)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a =>
                    a.Owner!.OwnerName.Contains(search) ||
                    (a.Pet != null && a.Pet.PetName.Contains(search))
                );
            }

            int totalRecords = await query.CountAsync();

            var appointments = await query
                .OrderBy(a => a.AppointmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (appointments, totalRecords);
        }

        public async Task<bool> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            var cancelledStatus = await _context.States.FirstOrDefaultAsync(s => s.StateName == AppointmentStateNames.Cancelled);
            if (cancelledStatus == null)
                throw new InvalidOperationException($"El estado '{AppointmentStateNames.Cancelled}' no existe en la base de datos. Verifica el seed de States.");

            appointment.StateId = cancelledStatus.IdState;
            appointment.Observation = "Cita cancelada";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAppointment(int id, DateTime newDate, string newObservation)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null) return false;

            appointment.AppointmentDate = newDate;
            appointment.Observation = newObservation;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ChangeAppointmentState(int id, int stateId)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            bool stateExists = await _context.States.AnyAsync(s => s.IdState == stateId);
            if (!stateExists) return false;

            appointment.StateId = stateId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Appointment>> GetByPetIdAsync(int petId)
        {
            return await _context.Appointments
                .Where(a => a.PetId == petId)
                .Include(a => a.Owner)
                .Include(a => a.State)
                .Include(a => a.Pet)
                    .ThenInclude(p => p!.Specie)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetBillableAppointmentsAsync(int ownerId)
        {
            return await (
                from a in _context.Appointments
                    .Include(a => a.State)
                    .Include(a => a.Pet)
                where a.OwnerId == ownerId
                    && a.State.StateName == AppointmentStateNames.Completed
                    && !_context.Invoices.Any(i => i.AppointmentId == a.Id)
                orderby a.AppointmentDate descending
                select a
            ).ToListAsync();
        }

        public async Task<List<Appointment>> GetScheduledAppointmentsForDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await (
                from a in _context.Appointments
                    .Include(a => a.Owner)
                    .Include(a => a.Pet)
                    .Include(a => a.State)
                where a.AppointmentDate >= startOfDay
                    && a.AppointmentDate < endOfDay
                    && a.State.StateName == AppointmentStateNames.Scheduled
                select a
            ).ToListAsync();
        }
    }
}
