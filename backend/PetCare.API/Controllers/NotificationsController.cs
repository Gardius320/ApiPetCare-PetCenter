using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Appointments.Commands.SendAppointmentReminders;

namespace PetCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public NotificationsController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("send-reminders")]
        public async Task<IActionResult> SendReminders([FromHeader(Name = "X-Reminder-Token")] string? token)
        {
            var expectedToken = _configuration["Notifications:ReminderToken"];

            if (string.IsNullOrEmpty(expectedToken) || token != expectedToken)
            {
                return Unauthorized();
            }

            var count = await _mediator.Send(new SendAppointmentRemindersCommand());
            return Ok(new { remindersSent = count });
        }
    }
}