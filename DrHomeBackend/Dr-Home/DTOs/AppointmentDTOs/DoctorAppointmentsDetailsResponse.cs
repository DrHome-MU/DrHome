namespace Dr_Home.DTOs.AppointmentDTOs
{
    public record DoctorAppointmentsDetailsResponse
    (
        Guid AppointmentId,
        string PatientName, 
        DateOnly? AppointmentDate, 
        TimeOnly AppointmentTime,
        string AppointmentDetails
     );
}
