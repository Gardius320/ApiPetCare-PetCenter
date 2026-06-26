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

            RuleFor(x => x.OwnerEmail)
                .EmailAddress()
                .WithMessage("El correo electrónico no es válido")
                .When(x => !string.IsNullOrEmpty(x.OwnerEmail));

            RuleFor(x => x.OwnerPhone)
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage("El número de teléfono no es válido")
                .When(x => !string.IsNullOrEmpty(x.OwnerPhone));

            RuleFor(x => x.Cedula)
                .Matches(@"^\d{6,10}$")
                .WithMessage("La cédula debe contener entre 6 y 10 dígitos")
                .When(x => !string.IsNullOrEmpty(x.Cedula));
        }
}
}
