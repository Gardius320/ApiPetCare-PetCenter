using MediatR;

namespace PetCare.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
