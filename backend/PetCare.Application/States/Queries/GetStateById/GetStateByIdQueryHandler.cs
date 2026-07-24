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
            var state = await _repository.GetById(request.Id);
           
            if (state == null) return null;
           
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
