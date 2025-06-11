namespace Dr_Home.Errors
{
    public static class AppointmentErrors
    {
        public static Error AppointmentConflict = new("Appointment.Conflict", "This Time Is already Booked",
          StatusCodes.Status409Conflict);

        public static Error AppointmentNotFound = new("Appointment.NotFound", "The Appointment Is Not Found",
          StatusCodes.Status404NotFound);

        public static Error UnauhorizedDetailsUpdate = new("Appointment.UnauhorizedDetailsUpdate", "Unauthorized Update",
         StatusCodes.Status401Unauthorized);

        public static Error CanNotUpdateDetails = new("Appointment.CanNotUpdateDetails", "Cannot Update Appointment Details While The Appointment Is Not Done",
        StatusCodes.Status400BadRequest);
    }
}
