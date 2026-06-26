using MediatR;
using PetCare.Domain.DTOs;

namespace PetCare.Application.Pets.Queries.GetPetById
{
    public class GetPetByIdQuery : IRequest<GetPetDTO?>
    {
       
        public int Id { get; set; }
    }
}
