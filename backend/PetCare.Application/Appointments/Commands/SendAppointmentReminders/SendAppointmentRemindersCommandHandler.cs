using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.SendAppointmentReminders
{
    public class SendAppointmentRemindersCommandHandler
        : IRequestHandler<SendAppointmentRemindersCommand, int>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IEmailService _emailService;

        public SendAppointmentRemindersCommandHandler(
            IAppointmentRepository appointmentRepository,
            IEmailService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _emailService = emailService;
        }

        public async Task<int> Handle(SendAppointmentRemindersCommand request, CancellationToken cancellationToken)
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var appointments = await _appointmentRepository.GetScheduledAppointmentsForDateAsync(tomorrow);

            int sentCount = 0;

            foreach (var appointment in appointments)
            {
                if (appointment.Owner == null || appointment.Pet == null)
                    continue;

                await _emailService.SendAppointmentReminderAsync(
                    appointment.Owner.Email,
                    appointment.Owner.OwnerName,
                    appointment.Pet.PetName,
                    appointment.AppointmentDate,
                    cancellationToken
                );

                sentCount++;
            }

            return sentCount;
        }
    }
}