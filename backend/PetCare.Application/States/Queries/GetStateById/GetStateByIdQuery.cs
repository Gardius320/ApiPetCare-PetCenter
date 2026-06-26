// Query para buscar un estado específico por su Id
using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.States.Queries.GetSateById
{
    public class GetStateByIdQuery : IRequest<GetStatesDTO?>
    {
        // Id del estado que se quiere consultar
        public int Id { get; set; }
    }
}
