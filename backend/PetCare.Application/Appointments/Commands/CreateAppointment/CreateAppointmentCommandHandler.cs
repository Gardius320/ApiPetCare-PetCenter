using MediatR;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler
        : IRequestHandler<CreateAppointmentCommand, int?>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPetRepository _petRepository;
        private readonly IEmailService _emailService;

        public CreateAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IOwnerRepository ownerRepository,
            IPetRepository petRepository,
            IEmailService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _ownerRepository = ownerRepository;
            _petRepository = petRepository;
            _emailService = emailService;
        }

        public async Task<int?> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.CreateAppointment(
                request.OwnerId,
                request.AppointmentDate,
                request.Observation ?? string.Empty,
                request.PetId
            );

            if (appointment != null && request.PetId.HasValue)
            {
                var owner = await _ownerRepository.GetByIdAsync(request.OwnerId);
                var pet = await _petRepository.GetByIdAsync(request.PetId.Value);

                if (owner != null && pet != null)
                {
                    await _emailService.SendAppointmentConfirmationAsync(
                        owner.Email,
                        owner.OwnerName,
                        pet.PetName,
                        request.AppointmentDate,
                        cancellationToken
                    );
                }
            }

            return appointment?.Id;
        }
    }
}