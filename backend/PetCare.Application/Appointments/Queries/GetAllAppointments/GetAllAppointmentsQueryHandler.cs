using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Queries.GetAllAppointments
{
    
    public class PaginatedAppointmentsResult
    {
        public List<GetAppointmentDTO> Items { get; set; } = new();
        public int TotalRecords { get; set; }
    }

    public class GetAllAppointmentsQueryHandler
        : IRequestHandler<GetAllAppointmentsQuery, PaginatedAppointmentsResult>
    {
        private readonly IAppointmentRepository _repository;

        public GetAllAppointmentsQueryHandler(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedAppointmentsResult> Handle(
            GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
         
            var (appointments, totalRecords) = await _repository.GetAllPagesAsync(
                request.Page, request.PageSize, request.Search);

            
            var items = new List<GetAppointmentDTO>();
            foreach (var a in appointments)
            {
                var dto = new GetAppointmentDTO
                {
                    Id           = a.Id,
                    Fecha        = a.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                    NombreDueno  = a.Owner?.OwnerName ?? string.Empty,
                    Estado       = a.State?.StateName ?? string.Empty,
                    Observacion  = a.Observation,
                    NombreMascota = a.Pet?.PetName ?? string.Empty,
                    Especie      = a.Pet?.Specie?.SpecieName ?? string.Empty
                };
                items.Add(dto);
            }

           
            return new PaginatedAppointmentsResult
            {
                Items = items,
                TotalRecords = totalRecords
            };
        }
    }
}
