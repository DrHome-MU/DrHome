namespace Dr_Home.DTOs.AppointmentDTOs
{
    public record PatientAppointmentsDetailsResponse
    (
        string DoctorName,
        DateOnly? AppointmentDate,
        TimeOnly AppointmentTime,
        string AppointmentDetails
    );
}
