using MediatR;
using PetCare.Application.Common;
using PetCare.Application.MedicalRecords.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.MedicalRecord.Queries.GetMedicalRecordById
{
    public class GetMedicalRecordByIdQueryHandler
        : IRequestHandler<GetMedicalRecordQuery, ApiResponse<MedicalRecordDTO>>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public GetMedicalRecordByIdQueryHandler(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<ApiResponse<MedicalRecordDTO>> Handle(
            GetMedicalRecordQuery request, CancellationToken cancellationToken)
        {
            var mr = await _medicalRecordRepository.GetByIdAsync(request.Id);
            if (mr == null)
                return ApiResponse<MedicalRecordDTO>.Failure("No se encontró la historia clínica.");

            var dto = new MedicalRecordDTO
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
            };

            return ApiResponse<MedicalRecordDTO>.Success(dto, "Historia clínica obtenida exitosamente.");
        }
    }
}
