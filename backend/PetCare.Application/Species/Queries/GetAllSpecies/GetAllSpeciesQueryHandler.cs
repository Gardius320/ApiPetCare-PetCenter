using MediatR;
using PetCare.Domain.DTOs;
using PetCare.Domain.Interfaces;

namespace PetCare.Application.Species.Queries.GetAllSpecies
{
    public class GetAllSpeciesQueryHandler
        : IRequestHandler<GetAllSpeciesQuery, List<GetSpeciesDTO>>
    {
        private readonly ISpeciesRepository _speciesRepository;

        public GetAllSpeciesQueryHandler(ISpeciesRepository speciesRepository)
        {
            _speciesRepository = speciesRepository;
        }

        public async Task<List<GetSpeciesDTO>> Handle(
            GetAllSpeciesQuery request, CancellationToken cancellationToken)
        {
            var species = await _speciesRepository.GetAll();
            
            return species.Select(s => new GetSpeciesDTO
            {
                Id = s.Id,
                SpeciesName = s.SpecieName
            }).ToList();
        }
    }
}