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
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public GetAllAppointmentsQueryHandler(IAppointmentRepository repository, IMedicalRecordRepository medicalRecordRepository)
        {
            _repository = repository;
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<PaginatedAppointmentsResult> Handle(
            GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            if (request.PetId.HasValue)
            {
                var usedIds = await _medicalRecordRepository.GetUsedAppointmentIdsAsync(request.PetId.Value);

                var appointments = await _repository.GetByPetIdAsync(request.PetId.Value);

                var filtered = appointments.Where(a => !usedIds.Contains(a.Id)).ToList();

                var items = new List<GetAppointmentDTO>();
                foreach (var a in filtered)
                {
                    var dto = new GetAppointmentDTO
                    {
                        Id = a.Id,
                        Date = a.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                        OwnerId = a.OwnerId,
                        OwnerName = a.Owner?.OwnerName ?? string.Empty,
                        State = a.State?.StateName ?? string.Empty,
                        Observation = a.Observation,
                        PetName = a.Pet?.PetName ?? string.Empty,
                        Species = a.Pet?.Specie?.SpecieName ?? string.Empty
                    };
                    items.Add(dto);
                }

                return new PaginatedAppointmentsResult
                {
                    Items = items,
                    TotalRecords = filtered.Count
                };
            }
            else
            {
                var (appointments, totalRecords) = await _repository.GetAllPagesAsync(
                    request.Page, request.PageSize, request.Search);

                var items = new List<GetAppointmentDTO>();
                foreach (var a in appointments)
                {
                    var dto = new GetAppointmentDTO
                    {
                        Id = a.Id,
                        Date = a.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                        OwnerId = a.OwnerId,
                        OwnerName = a.Owner?.OwnerName ?? string.Empty,
                        State = a.State?.StateName ?? string.Empty,
                        Observation = a.Observation,
                        PetName = a.Pet?.PetName ?? string.Empty,
                        Species = a.Pet?.Specie?.SpecieName ?? string.Empty
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
}