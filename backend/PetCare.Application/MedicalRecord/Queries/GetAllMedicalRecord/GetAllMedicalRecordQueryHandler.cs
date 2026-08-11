using MediatR;
using PetCare.Application.MedicalRecords.DTOs;
using PetCare.Domain.Interfaces;
using static PetCare.Application.MedicalRecord.Queries.GetAllMedicalRecord.GetAllMedicalRecordQuery;

namespace PetCare.Application.MedicalRecord.Queries.GetAllMedicalRecord
{
    public class GetAllMedicalRecordQueryHandler
        : IRequestHandler<GetAllMedicalRecordQuery, List<MedicalRecordDTO>>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public GetAllMedicalRecordQueryHandler(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<List<MedicalRecordDTO>> Handle(
            GetAllMedicalRecordQuery request, CancellationToken cancellationToken)
        {
            var medicalRecords = await _medicalRecordRepository.GetAllAsync(
                request.From, request.To, request.VetId);

            var items = new List<MedicalRecordDTO>();
            foreach (var mr in medicalRecords)
            {
                items.Add(new MedicalRecordDTO
                {
                    Id = mr.Id,
                    PetId = mr.PetId,
                    AppointmentId = mr.AppointmentId,
                    VeterinarianUserId = mr.VeterinarianUserId,
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