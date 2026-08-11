using MediatR;
using PetCare.Application.Common;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerCommandHandler
        : IRequestHandler<DeleteOwnerCommand, ApiResponse<string>>
    {
        private readonly IOwnerRepository _ownerRepository;

        public DeleteOwnerCommandHandler(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<ApiResponse<string>> Handle(DeleteOwnerCommand command, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByIdAsync(command.Id);

            if (owner is null)
                return ApiResponse<string>.Failure("Propietario no encontrado");

            var (petsCount, appointmentsCount) = await _ownerRepository.GetDependencyCountsAsync(command.Id);

            if (petsCount > 0 || appointmentsCount > 0)
            {
                return ApiResponse<string>.Failure(
                    $"No se puede eliminar al propietario porque tiene {petsCount} mascota(s) y {appointmentsCount} cita(s) asociadas."
                );
            }

            var mensaje = await _ownerRepository.DeleteOwner(command.Id);
            return ApiResponse<string>.Success(mensaje, mensaje);
        }
    }
}