using FluentValidation;
using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.Interfaces;
using MedicalRecordModel = PetCare.Domain.Models.MedicalRecord;

namespace PetCare.Application.MedicalRecord.Commands.Create
{
    public class CreateCommand : IRequest<int>
    {
        public int PetId { get; set; }
        public int? AppointmentId { get; set; }
        public required string VeterinarianUserId { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;
        public required string Diagnosis { get; set; }
        public required string Treatment { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Temperature { get; set; }
        public string? Observation { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
    }

    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.PetId)
                .NotEmpty().WithMessage("El Id de la mascota es obligatorio.");
            RuleFor(x => x.VeterinarianUserId)
                .NotEmpty().WithMessage("El Id del veterinario es obligatorio.");
            RuleFor(x => x.VisitDate)
                .NotEmpty().WithMessage("La fecha de la visita es obligatoria.");
            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("El diagnóstico es obligatorio.")
                .MaximumLength(500).WithMessage("El diagnóstico no puede exceder los 500 caracteres.");
            RuleFor(x => x.Treatment)
                .NotEmpty().WithMessage("El tratamiento es obligatorio.")
                .MaximumLength(500).WithMessage("El tratamiento no puede exceder los 500 caracteres.");
            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("El peso debe ser mayor o igual a cero.");
            RuleFor(x => x.Temperature)
                .GreaterThanOrEqualTo(0).WithMessage("La temperatura debe ser mayor o igual a cero.");
            RuleFor(x => x.Observation)
                .MaximumLength(500).WithMessage("La observación no puede exceder los 500 caracteres.");
        }
    }

    public class CreateCommandHandler : IRequestHandler<CreateCommand, int>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public CreateCommandHandler(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<int> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var medicalRecord = new MedicalRecordModel
            {
                PetId = request.PetId,
                AppointmentId = request.AppointmentId,
                VeterinarianUserId = request.VeterinarianUserId,
                VisitDate = request.VisitDate,
                Diagnopsis = request.Diagnosis,
                Treatment = request.Treatment,
                Weight = request.Weight,
                Temperature = request.Temperature,
                Observation = request.Observation,
                NextFollowUpDate = request.NextFollowUpDate
            };

            await _medicalRecordRepository.CreateMedicalRecordAsync(medicalRecord);
            return medicalRecord.Id;
        }
    }

    public class UpdateCommand : IRequest<ApiResponse<MedicalRecordModel>>
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int? AppointmentId { get; set; }
        public required string VeterinarianUserId { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;
        public required string Diagnosis { get; set; }
        public required string Treatment { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Temperature { get; set; }
        public string? Observation { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
    }

    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.PetId)
                .NotEmpty().WithMessage("El Id de la mascota es obligatorio.");
            RuleFor(x => x.VeterinarianUserId)
                .NotEmpty().WithMessage("El Id del veterinario es obligatorio.");
            RuleFor(x => x.VisitDate)
                .NotEmpty().WithMessage("La fecha de la visita es obligatoria.");
            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("El diagnóstico es obligatorio.")
                .MaximumLength(500).WithMessage("El diagnóstico no puede exceder los 500 caracteres.");
            RuleFor(x => x.Treatment)
                .NotEmpty().WithMessage("El tratamiento es obligatorio.")
                .MaximumLength(500).WithMessage("El tratamiento no puede exceder los 500 caracteres.");
            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("El peso debe ser mayor o igual a cero.");
            RuleFor(x => x.Temperature)
                .GreaterThanOrEqualTo(0).WithMessage("La temperatura debe ser mayor o igual a cero.");
            RuleFor(x => x.Observation)
                .MaximumLength(500).WithMessage("La observación no puede exceder los 500 caracteres.");
        }
    }

    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ApiResponse<MedicalRecordModel>>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public UpdateCommandHandler(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<ApiResponse<MedicalRecordModel>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var existing = await _medicalRecordRepository.GetByIdAsync(request.Id);
            if (existing == null)
                return ApiResponse<MedicalRecordModel>.Failure("No se encontró la historia clínica.");

            existing.PetId = request.PetId;
            existing.AppointmentId = request.AppointmentId;
            existing.VeterinarianUserId = request.VeterinarianUserId;
            existing.VisitDate = request.VisitDate;
            existing.Diagnopsis = request.Diagnosis;
            existing.Treatment = request.Treatment;
            existing.Weight = request.Weight;
            existing.Temperature = request.Temperature;
            existing.Observation = request.Observation;
            existing.NextFollowUpDate = request.NextFollowUpDate;

            var updated = await _medicalRecordRepository.UpdateMedicalRecordAsync(existing);
            return ApiResponse<MedicalRecordModel>.Success(updated!, "Historia clínica actualizada exitosamente.");
        }
    }

    public class DeleteCommand : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
    }

    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ApiResponse<string>>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public DeleteCommandHandler(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<ApiResponse<string>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var existing = await _medicalRecordRepository.GetByIdAsync(request.Id);
            if (existing == null)
                return ApiResponse<string>.Failure("No se encontró la historia clínica.");

            var result = await _medicalRecordRepository.DeleteMedicalRecordAsync(request.Id);
            return ApiResponse<string>.Success(result, "Historia clínica eliminada exitosamente.");
        }
    }
}