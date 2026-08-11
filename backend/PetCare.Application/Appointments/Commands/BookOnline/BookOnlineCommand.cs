using MediatR;

namespace PetCare.Application.Appointments.Commands.BookOnline
{
    public class BookOnlineCommand : IRequest<int?>
    {
        public string? OwnerName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string? Observation { get; set; }

        public string? PetName { get; set; }

        public int SpecieId { get; set; }
    }
}