using MediatR;

namespace PetCare.Application.Appointments.Queries.GetAvailableSlots
{
    public class GetAvailableSlotsQuery : IRequest<List<string>>
    {
        public DateTime Date { get; set; }
    }
}
