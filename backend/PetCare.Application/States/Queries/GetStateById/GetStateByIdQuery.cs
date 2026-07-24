using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.States.Queries.GetSateById
{
    public class GetStateByIdQuery : IRequest<GetStatesDTO?>
    {
      
        public int Id { get; set; }
    }
}
