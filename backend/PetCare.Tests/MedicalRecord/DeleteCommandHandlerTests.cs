using Moq;
using PetCare.Application.MedicalRecord.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.MedicalRecord
{
    public class DeleteCommandHandlerTests
    {
        private readonly Mock<IMedicalRecordRepository> _medicalRecordRepository;
        private readonly DeleteCommandHandler _handler;

        public DeleteCommandHandlerTests()
        {
            _medicalRecordRepository = new Mock<IMedicalRecordRepository>();
            _handler = new DeleteCommandHandler(_medicalRecordRepository.Object);
        }

        [Fact]
        public async Task Handle_RecordDoesNotExist_ReturnsFailure()
        {
            _medicalRecordRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((PetCare.Domain.Models.MedicalRecord?)null);

            var command = new DeleteCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _medicalRecordRepository.Verify(r => r.DeleteMedicalRecordAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RecordExists_DeletesSuccessfully()
        {
            _medicalRecordRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new PetCare.Domain.Models.MedicalRecord { Id = 1 });
            _medicalRecordRepository
                .Setup(r => r.DeleteMedicalRecordAsync(1))
                .ReturnsAsync("Eliminado");

            var command = new DeleteCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _medicalRecordRepository.Verify(r => r.DeleteMedicalRecordAsync(1), Times.Once);
        }
    }
}
