using FluentValidation;

namespace PetCare.Application.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private static readonly string[] RolesPermitidos = { "Admin", "Veterinario", "Cliente", "Auxiliar" };

        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("El nombre es obligatorio");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("El apellido es obligatorio");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El formato del email no es válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("El rol es obligatorio")
                .Must(rol => RolesPermitidos.Contains(rol))
                .WithMessage($"Rol inválido. Los roles permitidos son: {string.Join(", ", RolesPermitidos)}");
        }
    }
}