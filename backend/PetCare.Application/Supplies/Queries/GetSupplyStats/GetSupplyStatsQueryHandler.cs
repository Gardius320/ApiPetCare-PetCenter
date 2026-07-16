using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Supplies.Queries.GetSupplyStats
{
    public class GetSupplyStatsQueryHandler : IRequestHandler<GetSupplyStatsQuery, SupplyStatsDTO>
    {
        private readonly ISupplyRepository _repository;

        public GetSupplyStatsQueryHandler(ISupplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<SupplyStatsDTO> Handle(GetSupplyStatsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetStatsAsync();
        }
    }
}