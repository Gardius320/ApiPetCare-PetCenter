using FluentValidation;
using MediatR;


namespace PetCare.Application.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommandValidator : AbstractValidator<CreateOwnerCommand>
    {

        public CreateOwnerCommandValidator() 
        {

            RuleFor(x => x.OwnerName)
                .NotEmpty()
                .WithMessage("El nombre del dueño es obligatorio");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("El correo electrónico es obligatorio y debe ser válido");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(10)
                .WithMessage("El número de telefono debe tener los caracteres correspondientes");

            RuleFor(x => x.IdCard)
                .NotEmpty()
                .MinimumLength(7)
                .MaximumLength(10)
                .WithMessage("La cédula debe tener entre 7 y 10 caracteres");



        }
    }
}
