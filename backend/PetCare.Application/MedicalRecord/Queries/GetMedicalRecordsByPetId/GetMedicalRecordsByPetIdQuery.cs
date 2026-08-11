using MediatR;
using PetCare.Application.MedicalRecords.DTOs;

namespace PetCare.Application.MedicalRecord.Queries.GetMedicalRecordsByPetId
{
    public class GetMedicalRecordsByPetIdQuery : IRequest<List<MedicalRecordDTO>>
    {
        public int PetId { get; set; }
    }
}
