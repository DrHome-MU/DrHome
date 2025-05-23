namespace Dr_Home.DTOs.AppointmentDTOs
{
    public record AppointmentResponse
    (
        Guid Id ,
        TimeOnly AppointmentTime,
        string PatientName,
        string PatientPhoneNumber,
        bool IsActive,
        bool IsDone,
        Guid? DoctorId, 
        Guid? PatientId,
        Guid? ScheduleId
        );
}
