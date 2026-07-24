using MediatR;

namespace PetCare.Application.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }


}
