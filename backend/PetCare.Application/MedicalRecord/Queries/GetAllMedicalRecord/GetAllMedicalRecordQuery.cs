using MediatR;
using PetCare.Application.MedicalRecords.DTOs;

namespace PetCare.Application.MedicalRecord.Queries.GetAllMedicalRecord
{
    public class GetAllMedicalRecordQuery : IRequest<List<MedicalRecordDTO>>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? VetId { get; set; }
    }
}