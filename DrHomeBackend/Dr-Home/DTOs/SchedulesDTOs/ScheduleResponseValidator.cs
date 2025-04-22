using FluentValidation;

namespace Dr_Home.DTOs.SchedulesDTOs
{
    public class ScheduleResponseValidator : AbstractValidator<ScheduleResponse>
    {
        public ScheduleResponseValidator()
        {
            //WorkDay
            RuleFor(x => x.WorkDay)
                .NotEmpty()
                .WithMessage("The Work Day Is Required")
                .Must(ValidateWorkDay)
                .WithMessage("Please Enter Valid Date");

            //StartTime 

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .Must(ValidateTime)
                .WithMessage("The Hour Must be between 0 and 23");

            //EndTime

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .Must(ValidateTime)
                .WithMessage("The Hour Must be between 0 and 23");

            //Comparesation

            RuleFor(x => x.StartTime)
           .LessThan(x => x.EndTime)
           .WithMessage("Start Time Must be less than End Time");

            //Fee 

            RuleFor(x => x.Fee)
                .NotEmpty()
                .Must(ValidateFee)
                .WithMessage("Fee Must be >= 0");
            //AppointmentDuration
            //AppointmentDuration
            RuleFor(x => x.AppointmentDurationInMiniutes)
                .NotEmpty()
                .Must(ValidateDuration)
                .WithMessage("Must Be Graeter Than 5 mins");

            RuleFor(x => x)
                .Must(ValidateTimes)
                .WithMessage("StartTime and EndTime are not valid");
        }

        private bool ValidateWorkDay(DateOnly WorkDay)
        {
            return WorkDay >= DateOnly.FromDateTime(DateTime.UtcNow);
        }
        private bool ValidateTime(TimeOnly timeOnly)
        {
            int hour = (int)timeOnly.Hour;

            return (hour >= 0 && hour <= 23);
        }
        private bool ValidateFee(decimal fee)
        {
            return fee >= 0;
        }
        private bool ValidateDuration(int duration)
        {
            return duration >= 5;
        }

        private bool ValidateTimes(ScheduleResponse request)
        {
            if (request.WorkDay == DateOnly.FromDateTime(DateTime.UtcNow) &&
                request.StartTime.Hour <= TimeOnly.FromDateTime(DateTime.UtcNow).Hour)
                return false;
            return !(request.StartTime.AddMinutes(request.AppointmentDurationInMiniutes) > request.EndTime);
        }
    }
}

