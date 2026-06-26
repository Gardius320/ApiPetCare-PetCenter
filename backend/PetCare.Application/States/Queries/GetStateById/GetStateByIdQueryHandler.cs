// Handler que busca un estado por Id y lo convierte a DTO
using MediatR;
using PetCare.Application.States.Queries.GetSateById;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.States.Queries.GetStateById
{
    public class GetStateByIdQueryHandler : IRequestHandler<GetStateByIdQuery, GetStatesDTO?>
    {
        private readonly IStateRepository _repository;

        public GetStateByIdQueryHandler(IStateRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetStatesDTO?> Handle(GetStateByIdQuery request, CancellationToken ct)
        {
            // Buscamos el estado en la base de datos
            var state = await _repository.GetById(request.Id);

            // Si no existe, devolvemos null para que el controlador responda 404
            if (state == null) return null;

            // Construimos el DTO con los datos del estado
            var dto = new GetStatesDTO
            {
                IdState     = state.IdState,
                StateName   = state.StateName,
                Description = state.Description
            };

            return dto;
        }
    }
}
