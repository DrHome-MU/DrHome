namespace Dr_Home.DTOs.SchedulesDTOs
{
    public class ScheduleResponse
    {
        public Guid Id { get; set; }
        public DateOnly WorkDay { get; set; }

         public TimeOnly StartTime { get; set; }
         public TimeOnly EndTime {  get; set; } 

        public  int AppointmentDurationInMiniutes {  get; set; }

        public IDictionary<TimeOnly, bool> ValidTimesForBooking { get; set; } = new Dictionary<TimeOnly, bool>();

        public  decimal Fee { get; set; }

    }
}

