namespace PetCare.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendAppointmentConfirmationAsync(
            string toEmail,
            string ownerName,
            string petName,
            DateTime appointmentDate,
            CancellationToken cancellationToken = default);

        Task SendAppointmentReminderAsync(
            string toEmail,
            string ownerName,
            string petName,
            DateTime appointmentDate,
            CancellationToken cancellationToken = default);
    }
}