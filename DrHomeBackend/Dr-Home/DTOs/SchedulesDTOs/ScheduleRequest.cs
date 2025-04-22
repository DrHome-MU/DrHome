namespace Dr_Home.DTOs.SchedulesDTOs
{
    public record ScheduleRequest
    (
     DateOnly WorkDay,

    TimeOnly StartTime,
    TimeOnly EndTime,

    int AppointmentDurationInMiniutes,

    

    decimal Fee
        );
}
