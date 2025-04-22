namespace Dr_Home.Errors
{
    public static class AppointmentErrors
    {
        public static Error AppointmentConflict = new("Appointment.Conflict", "This Time Is already Booked",
          StatusCodes.Status409Conflict);

        public static Error AppointmentNotFound = new("Appointment.NotFound", "The Appointment Is Not Found",
          StatusCodes.Status404NotFound);
    }
}
