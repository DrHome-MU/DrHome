namespace Dr_Home.Errors
{
    public static class ScheduleErrors
    {
        public static Error ScheduleConflict = new("Schedule.Conflict", "This Data Conflict With Existed Schedule",
            StatusCodes.Status409Conflict);

        public static Error ScheduleNotFound = new("Schedule.NotFound", "This Schedule Is Not Found",
            StatusCodes.Status404NotFound);

        public static Error UnauthorizedAdding = new("Schedule.UnauthorizedAdding", "This Doctor Has No Right To Add Schedule In This Clinic",
           StatusCodes.Status401Unauthorized);

        public static Error ScheduleCannotBeDeleted = new("Schedule.ScheduleCannotBeDeleted", "This schedcule already has Booked Appointments and can`t be deleted",
           StatusCodes.Status400BadRequest);
    }
}
