
using MediatR;

namespace PetCare.Application.States.Commands.UpdateState
{
    public class UpdateStateCommand : IRequest<int?>
    {
       
        public int Id { get; set; }       
        public string StateName { get; set; } = null!;       
        public string? StateDescription { get; set; }
    }
}
