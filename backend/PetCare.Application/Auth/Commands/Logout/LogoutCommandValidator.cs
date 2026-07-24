using FluentValidation;

namespace PetCare.Application.Auth.Commands.Logout
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {

        public LogoutCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
