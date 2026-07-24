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
            var states = await _stateRepository.GetAll();
            
            var list = new List<GetStatesDTO>();
            foreach (var s in states)
            {
                var dto = new GetStatesDTO
                {
                    IdState     = s.IdState,
                    StateName   = s.StateName,
                    Description = s.Description
                };
                list.Add(dto);
            }

            return list;
        }
    }
}