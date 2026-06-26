using Microsoft.EntityFrameworkCore;
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

        public async Task<Appointment?> CreateAppointment(int ownerId, DateTime appointmentDate, string description, int? petId = null)
        {
            bool ownerExists = await _context.Owners.AnyAsync(o => o.Id == ownerId);
            if (!ownerExists) return null;

            var appointment = new Appointment
            {
                OwnerId         = ownerId,
                AppointmentDate = appointmentDate,
                Observation     = description,
                StateId         = 1,
                PetId           = petId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
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

            appointment.StateId    = 3;
            appointment.Observation = "Cita cancelada";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAppointment(int id, DateTime newDate, string newObservation)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null) return false;

            appointment.AppointmentDate = newDate;
            appointment.Observation     = newObservation;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
