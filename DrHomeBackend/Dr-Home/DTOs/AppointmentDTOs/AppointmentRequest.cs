namespace Dr_Home.DTOs.AppointmentDTOs
{
    public record AppointmentRequest
    (
          Guid PatientId,
          string PatientName,

         string PatientPhoneNumber,

        TimeOnly AppointmentTime
      );
}
