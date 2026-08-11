using MediatR;
using PetCare.Domain.Constants;
using PetCare.Domain.Interfaces;
using PetCare.Domain.Models;

namespace PetCare.Application.Appointments.Commands.BookOnline
{
    public class BookOnlineCommandHandler : IRequestHandler<BookOnlineCommand, int?>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPetRepository _petRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public BookOnlineCommandHandler(
            IOwnerRepository ownerRepository,
            IPetRepository petRepository,
            IAppointmentRepository appointmentRepository)
        {
            _ownerRepository = ownerRepository;
            _petRepository = petRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<int?> Handle(BookOnlineCommand request, CancellationToken cancellationToken)
        {
            var existingOwner = await _ownerRepository.GetByEmailAsync(request.Email!);

            Owner ownerToUse;
            if (existingOwner != null)
            {
                ownerToUse = existingOwner;
            }
            else
            {
                ownerToUse = new Owner
                {
                    OwnerName = request.OwnerName!,
                    Email = request.Email!,
                    PhoneNumber = request.PhoneNumber,
                    Gender = request.Gender,
                    IsActive = true
                };

                var createdOwner = await _ownerRepository.CreateOwner(ownerToUse);
                if (createdOwner is null)
                    return null;

                ownerToUse = createdOwner;
            }

            var pet = new Pet
            {
                PetName = request.PetName!,
                SpecieId = request.SpecieId,
                OwnerId = ownerToUse.Id,
                IsActive = true
            };

            var createdPet = await _petRepository.CreatePetAsync(pet);
            if (createdPet is null)
                return null;

            var appointment = await _appointmentRepository.CreateAppointment(
                ownerToUse.Id,
                request.AppointmentDate,
                request.Observation ?? string.Empty,
                petId: createdPet.Id,
                stateName: AppointmentStateNames.PendingConfirmation
            );

            return appointment?.Id;
        }
    }
}