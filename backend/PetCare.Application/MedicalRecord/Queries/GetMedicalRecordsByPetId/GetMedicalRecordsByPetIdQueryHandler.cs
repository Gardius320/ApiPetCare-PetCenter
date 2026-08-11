using MediatR;
using PetCare.Application.MedicalRecords.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.MedicalRecord.Queries.GetMedicalRecordsByPetId
{
    public class GetMedicalRecordsByPetIdQueryHandler
        : IRequestHandler<GetMedicalRecordsByPetIdQuery, List<MedicalRecordDTO>>
    {
        private readonly IMedicalRecordRepository _repository;

        public GetMedicalRecordsByPetIdQueryHandler(IMedicalRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MedicalRecordDTO>> Handle(
            GetMedicalRecordsByPetIdQuery request, CancellationToken cancellationToken)
        {
            var medicalRecords = await _repository.GetByPetIdAsync(request.PetId);

            var items = new List<MedicalRecordDTO>();
            foreach (var mr in medicalRecords)
            {
                items.Add(new MedicalRecordDTO
                {
                    Id = mr.Id,
                    PetId = mr.PetId,
                    PetName = mr.Pet.PetName,
                    AppointmentId = mr.AppointmentId,
                    VeterinarianUserId = mr.VeterinarianUserId,
                    VeterinarianName = mr.Veterinarian.FullName,
                    VisitDate = mr.VisitDate,
                    Diagnosis = mr.Diagnopsis,
                    Treatment = mr.Treatment,
                    Weight = mr.Weight,
                    Temperature = mr.Temperature,
                    Observations = mr.Observation,
                    NextFollowUpDate = mr.NextFollowUpDate
                });
            }
            return items;
        }
    }
}
