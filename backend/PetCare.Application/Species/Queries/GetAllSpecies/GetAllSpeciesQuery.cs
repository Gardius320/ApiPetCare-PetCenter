using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Species.Queries.GetAllSpecies
{

    public class GetAllSpeciesQuery : IRequest<List<GetSpeciesDTO>>
    {
    }
}
