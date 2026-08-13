using Moq;
using PetCare.Application.MedicalRecord.Commands.Create;
using PetCare.Domain.Interfaces;

namespace PetCare.Tests.MedicalRecord
{
    public class CreateCommandHandlerTests
    {
        private readonly Mock<IMedicalRecordRepository> _medicalRecordRepository;
        private readonly CreateCommandHandler _handler;

        public CreateCommandHandlerTests()
        {
            _medicalRecordRepository = new Mock<IMedicalRecordRepository>();
            _handler = new CreateCommandHandler(_medicalRecordRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidRecord_ReturnsGeneratedId()
        {
            // El handler ignora el valor que devuelve el repositorio y retorna
            // el Id del objeto local. Igual que hace EF Core al insertar,
            // simulamos que el repositorio "asigna" el Id mutando el mismo objeto.
            _medicalRecordRepository
                .Setup(r => r.CreateMedicalRecordAsync(It.IsAny<PetCare.Domain.Models.MedicalRecord>()))
                .Callback<PetCare.Domain.Models.MedicalRecord>(record => record.Id = 5)
                .ReturnsAsync((PetCare.Domain.Models.MedicalRecord record) => record);

            var command = new CreateCommand
            {
                PetId = 1,
                VeterinarianUserId = "vet-1",
                Diagnosis = "Otitis",
                Treatment = "Limpieza y gotas"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task Handle_ValidRecord_MapsFieldsCorrectlyBeforeSaving()
        {
            _medicalRecordRepository
                .Setup(r => r.CreateMedicalRecordAsync(It.IsAny<PetCare.Domain.Models.MedicalRecord>()))
                .ReturnsAsync((PetCare.Domain.Models.MedicalRecord record) => record);

            var command = new CreateCommand
            {
                PetId = 3,
                AppointmentId = 10,
                VeterinarianUserId = "vet-9",
                Diagnosis = "Fractura",
                Treatment = "Enyesado",
                Weight = 12.5m,
                Temperature = 38.2m,
                Observation = "Reposo por dos semanas"
            };

            await _handler.Handle(command, CancellationToken.None);

            _medicalRecordRepository.Verify(r => r.CreateMedicalRecordAsync(It.Is<PetCare.Domain.Models.MedicalRecord>(m =>
                m.PetId == 3 &&
                m.AppointmentId == 10 &&
                m.VeterinarianUserId == "vet-9" &&
                m.Diagnopsis == "Fractura" &&
                m.Treatment == "Enyesado" &&
                m.Weight == 12.5m &&
                m.Temperature == 38.2m &&
                m.Observation == "Reposo por dos semanas")), Times.Once);
        }
    }
}
