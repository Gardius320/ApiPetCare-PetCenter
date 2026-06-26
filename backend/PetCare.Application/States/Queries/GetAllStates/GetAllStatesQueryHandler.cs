// Handler que consulta todos los estados y los convierte a DTO
using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.States.Queries.GetAllStates
{
    public class GetAllStatesQueryHandler : IRequestHandler<GetAllStatesQuery, List<GetStatesDTO>>
    {
        private readonly IStateRepository _stateRepository;

        public GetAllStatesQueryHandler(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<List<GetStatesDTO>> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
        {
            // Traemos todos los estados de la base de datos
            var states = await _stateRepository.GetAll();

            // Convertimos cada estado a DTO con un foreach
            var lista = new List<GetStatesDTO>();
            foreach (var s in states)
            {
                var dto = new GetStatesDTO
                {
                    IdState     = s.IdState,
                    StateName   = s.StateName,
                    Description = s.Description
                };
                lista.Add(dto);
            }

            return lista;
        }
    }
}