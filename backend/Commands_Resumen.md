# Commands — PetCare API

Archivo compilado con todos los Commands de la capa Application.

---

## Owners

### CreateOwnerCommand.cs

```csharp
// Datos que llegan del formulario para registrar un nuevo dueño
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommand : IRequest<int?>
    {
        // Nombre obligatorio, máximo 25 caracteres
        [Required(ErrorMessage = "El nombre del dueño es obligatorio")]
        [StringLength(25, ErrorMessage = "El nombre no puede superar los 25 caracteres")]
        public string? OwnerName { get; set; }

        // Email obligatorio con formato válido
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        public string? Email { get; set; }

        // Teléfono opcional pero con formato válido
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        public string? PhoneNumber { get; set; }

        // Cédula opcional, máximo 10 caracteres
        [StringLength(10, ErrorMessage = "La cédula no puede superar los 10 caracteres")]
        public string? Cedula { get; set; }

        // Género opcional
        public string? Gender { get; set; }
    }
}
```

---

### UpdateOwnerCommand.cs

```csharp
// Datos para actualizar la información de un dueño existente
using MediatR;

namespace PetCare.Application.Owners.Commands.UpdateOwner
{
    public class UpdateOwnerCommand : IRequest<int?>
    {
        // Id del dueño a actualizar
        public int OwnerId { get; set; }

        public string? OwnerName { get; set; }
        public string? OwnerEmail { get; set; }
        public string? OwnerPhone { get; set; }
        public string? Gender { get; set; }
        public string? Cedula { get; set; }
    }
}
```

---

## Pets

### CreatePetCommand.cs

```csharp
// Datos necesarios para registrar una nueva mascota
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.Pets.Commands.CreatePet
{
    public class CreatePetCommand : IRequest<int?>
    {
        // Nombre obligatorio entre 1 y 50 caracteres
        [Required(ErrorMessage = "El nombre de la mascota es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 50 caracteres")]
        public string? PetName { get; set; }

        // La especie debe ser válida (Id mayor a 0)
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una especie válida")]
        public int SpecieId { get; set; }

        // El dueño debe existir (Id mayor a 0)
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un dueño válido")]
        public int OwnerId { get; set; }
    }
}
```

---

### UpdatePetCommand.cs

```csharp
// Datos para modificar una mascota que ya existe
using MediatR;

namespace PetCare.Application.Pets.Commands.UpdatePet
{
    public class UpdatePetCommand : IRequest<int?>
    {
        // Id de la mascota que se quiere editar
        public int PetId { get; set; }

        // Nuevo nombre
        public string? PetName { get; set; }

        // Nueva especie
        public int SpecieId { get; set; }

        // Nuevo dueño
        public int OwnerId { get; set; }
    }
}
```

---

### ChangeStatePetCommand.cs

```csharp
// Comando para activar o desactivar una mascota
using MediatR;

namespace PetCare.Application.Pets.Commands.ChangeStatePet
{
    public class ChangeStatePetCommand : IRequest<int?>
    {
        // Id de la mascota cuyo estado queremos cambiar
        public int PetId { get; set; }

        // true = activa, false = inactiva
        public bool IsActive { get; set; }
    }
}
```

---

## Species

### CreateSpeciesCommand.cs

```csharp
// Comando para crear una nueva especie (ej: Perro, Gato, Ave)
using MediatR;

namespace PetCare.Application.Species.Commands.CreateSpecies
{
    public class CreateSpeciesCommand : IRequest<int?>
    {
        // Nombre de la especie a crear
        public string? SpecieName { get; set; }
    }
}
```

---

### DeleteSpeciesCommand.cs

```csharp
// Comando para eliminar una especie por su Id
using MediatR;

namespace PetCare.Application.Species.Commands.DeleteSpecies
{
    public class DeleteSpeciesCommand : IRequest<string>
    {
        // Id de la especie a eliminar
        public int Id { get; set; }
    }
}
```

---

## States

### CreateStatesCommand.cs

```csharp
// Comando para crear un nuevo estado (ej: Pendiente, Confirmada, Cancelada)
using MediatR;

namespace PetCare.Application.States.Commands.CreateStates
{
    public class CreateStatesCommand : IRequest<int?>
    {
        // Nombre del estado
        public string? StateName { get; set; }

        // Descripción opcional del estado
        public string? Description { get; set; }
    }
}
```

---

### DeleteStatesCommand.cs

```csharp
// Comando para eliminar un estado por su Id
using MediatR;

namespace PetCare.Application.States.Commands.DeleteStates
{
    public class DeleteStatesCommand : IRequest<int?>
    {
        // Id del estado a eliminar
        public int Id { get; set; }
    }
}
```

---

### UpdateStateCommand.cs

```csharp
// Datos para modificar un estado existente
using MediatR;

namespace PetCare.Application.States.Commands.UpdateState
{
    public class UpdateStateCommand : IRequest<int?>
    {
        // Id del estado a actualizar
        public int Id { get; set; }

        // Nuevo nombre del estado (obligatorio)
        public string StateName { get; set; } = null!;

        // Nueva descripción (opcional)
        public string? StateDescription { get; set; }
    }
}
```

---

## Appointments

### DeleteAppointmentCommand.cs

```csharp
// Comando para cancelar (eliminar) una cita por su Id
using MediatR;

namespace PetCare.Application.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommand : IRequest<int?>
    {
        // Id de la cita que se quiere cancelar
        public int Id { get; set; }
    }
}
```

---

### UpdateAppointmentCommand.cs

```csharp
// Datos necesarios para actualizar una cita existente
using MediatR;

namespace PetCare.Application.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommand : IRequest<int?>
    {
        // Id de la cita a modificar
        public int Id { get; set; }

        // Nueva fecha para la cita
        public DateTime AppointmentDate { get; set; }

        // Nueva observación (puede quedar vacía)
        public string? Observation { get; set; }
    }
}
```
