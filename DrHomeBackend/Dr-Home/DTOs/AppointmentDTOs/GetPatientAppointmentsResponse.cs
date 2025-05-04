namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class GetPatientAppointmentsResponse
    {
        public Guid AppointmentId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string ClinicName {  get; set; } = string.Empty;

        public string ClinicCity {  get; set; } = string.Empty;

        public string ClinicRegion { get;   set; } = string.Empty;

        public string? ClinicPhoneNumber {  get; set; }

        public DateOnly AppointmentDate {  get; set; }
        public TimeOnly AppointmentTime {  get; set; }


    }
}
