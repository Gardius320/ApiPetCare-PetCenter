using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.States.Queries.GetAllStates
{
   
    public class GetAllStatesQuery : IRequest<List<GetStatesDTO>>
    {
    }
}