namespace PetCare.Domain.DTOs
{
    public record BillableAppointmentDto(
        int Id,
        DateTime AppointmentDate,
        string PetName
    );
}