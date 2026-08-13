using Moq;
using PetCare.Application.MedicalRecord.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.MedicalRecord
{
    public class UpdateCommandHandlerTests
    {
        private readonly Mock<IMedicalRecordRepository> _medicalRecordRepository;
        private readonly UpdateCommandHandler _handler;

        public UpdateCommandHandlerTests()
        {
            _medicalRecordRepository = new Mock<IMedicalRecordRepository>();
            _handler = new UpdateCommandHandler(_medicalRecordRepository.Object);
        }

        [Fact]
        public async Task Handle_RecordDoesNotExist_ReturnsFailure()
        {
            _medicalRecordRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((PetCare.Domain.Models.MedicalRecord?)null);

            var command = new UpdateCommand
            {
                Id = 1,
                PetId = 1,
                VeterinarianUserId = "vet-1",
                Diagnosis = "Otitis",
                Treatment = "Gotas"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _medicalRecordRepository.Verify(r => r.UpdateMedicalRecordAsync(It.IsAny<PetCare.Domain.Models.MedicalRecord>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RecordExists_UpdatesSuccessfully()
        {
            var existing = new PetCare.Domain.Models.MedicalRecord
            {
                Id = 1,
                PetId = 1,
                VeterinarianUserId = "vet-1",
                Diagnopsis = "Otitis",
                Treatment = "Gotas"
            };

            _medicalRecordRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);
            _medicalRecordRepository
                .Setup(r => r.UpdateMedicalRecordAsync(It.IsAny<PetCare.Domain.Models.MedicalRecord>()))
                .ReturnsAsync((PetCare.Domain.Models.MedicalRecord record) => record);

            var command = new UpdateCommand
            {
                Id = 1,
                PetId = 1,
                VeterinarianUserId = "vet-1",
                Diagnosis = "Otitis crónica",
                Treatment = "Cirugía"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _medicalRecordRepository.Verify(r => r.UpdateMedicalRecordAsync(It.Is<PetCare.Domain.Models.MedicalRecord>(m =>
                m.Diagnopsis == "Otitis crónica" && m.Treatment == "Cirugía")), Times.Once);
        }
    }
}
