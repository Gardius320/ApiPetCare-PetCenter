using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Supplies.Queries.GetSupplyStats
{
    public class GetSupplyStatsQuery : IRequest<SupplyStatsDTO>
    {
    }
}