using FluentValidation;
using MediatR;



namespace PetCare.Application.Owners.Commands.UpdateOwner
{
    public class updateOwnerCommandValidator : AbstractValidator<UpdateOwnerCommand>
    {
        public updateOwnerCommandValidator()
        {
            RuleFor(x => x.OwnerId)
                .GreaterThan(0)
                .WithMessage("Debes seleccionar un dueño válido");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("El correo electrónico no es válido")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage("El número de teléfono no es válido")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.IdCard)
                .Matches(@"^\d{6,10}$")
                .WithMessage("La cédula debe contener entre 6 y 10 dígitos")
                .When(x => !string.IsNullOrEmpty(x.IdCard));
        }
}
}
