using MediatR;

namespace PetCare.Application.States.Commands.CreateStates
{
    public class CreateStatesCommand : IRequest<int?>
    {
   
        public string? StateName { get; set; }

        public string? Description { get; set; }
    }
}

