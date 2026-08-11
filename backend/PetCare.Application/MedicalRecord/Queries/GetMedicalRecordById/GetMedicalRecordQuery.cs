using MediatR;
using PetCare.Application.Common;
using PetCare.Application.MedicalRecords.DTOs;

namespace PetCare.Application.MedicalRecord.Queries.GetMedicalRecordById
{
    public class GetMedicalRecordQuery : IRequest<ApiResponse<MedicalRecordDTO>>
    {
        public int Id { get; set; }
    }
}
