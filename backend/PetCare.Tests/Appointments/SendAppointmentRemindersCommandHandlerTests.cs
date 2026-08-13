using Moq;
using PetCare.Application.Appointments.Commands.SendAppointmentReminders;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Tests.Appointments
{
    public class SendAppointmentRemindersCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
        private readonly Mock<IEmailService> _emailService = new();
        private readonly SendAppointmentRemindersCommandHandler _handler;

        public SendAppointmentRemindersCommandHandlerTests()
        {
            _handler = new SendAppointmentRemindersCommandHandler(_appointmentRepository.Object, _emailService.Object);
        }

        [Fact]
        public async Task Handle_NoAppointmentsScheduled_ReturnsZero()
        {
            _appointmentRepository
                .Setup(r => r.GetScheduledAppointmentsForDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Appointment>());

            var result = await _handler.Handle(new SendAppointmentRemindersCommand(), CancellationToken.None);

            Assert.Equal(0, result);
            _emailService.Verify(e => e.SendAppointmentReminderAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_AppointmentsMissingOwnerOrPet_AreSkipped()
        {
            var appointments = new List<Appointment>
            {
                new() { Id = 1, Owner = null, Pet = new Pet { PetName = "Firulais" } },
                new() { Id = 2, Owner = new Owner { OwnerName = "Jose", Email = "jose@petcare.com" }, Pet = null },
                new() { Id = 3, Owner = new Owner { OwnerName = "Ana", Email = "ana@petcare.com" }, Pet = new Pet { PetName = "Michi" } }
            };

            _appointmentRepository
                .Setup(r => r.GetScheduledAppointmentsForDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(appointments);

            var result = await _handler.Handle(new SendAppointmentRemindersCommand(), CancellationToken.None);

            Assert.Equal(1, result); // solo la cita 3 tiene Owner y Pet completos
            _emailService.Verify(e => e.SendAppointmentReminderAsync(
                "ana@petcare.com", "Ana", "Michi", It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidAppointments_SendsReminderForEachAndReturnsCount()
        {
            var appointmentDate = DateTime.Today.AddDays(1);
            var appointments = new List<Appointment>
            {
                new() { Id = 1, AppointmentDate = appointmentDate, Owner = new Owner { OwnerName = "Jose", Email = "jose@petcare.com" }, Pet = new Pet { PetName = "Firulais" } },
                new() { Id = 2, AppointmentDate = appointmentDate, Owner = new Owner { OwnerName = "Ana", Email = "ana@petcare.com" }, Pet = new Pet { PetName = "Michi" } }
            };

            _appointmentRepository
                .Setup(r => r.GetScheduledAppointmentsForDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(appointments);

            var result = await _handler.Handle(new SendAppointmentRemindersCommand(), CancellationToken.None);

            Assert.Equal(2, result);
            _emailService.Verify(e => e.SendAppointmentReminderAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), appointmentDate, It.IsAny<CancellationToken>()),
                Times.Exactly(2));
        }
    }
}
