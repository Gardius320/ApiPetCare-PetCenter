using MediatR;

namespace PetCare.Application.States.Commands.DeleteStates
{
    public class DeleteStatesCommand : IRequest<int?>
    {
      
        public int Id { get; set; }
    }
}
