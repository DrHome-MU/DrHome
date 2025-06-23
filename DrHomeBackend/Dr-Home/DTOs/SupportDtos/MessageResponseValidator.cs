namespace Dr_Home.DTOs.SupportDtos
{
    public class MessageResponseValidator:AbstractValidator<MessageResponse>
    {
        public MessageResponseValidator()
        {
            RuleFor(x => x.SenderName)
               .NotEmpty()
               .WithMessage("The Name Must be Not Empty")
               .NotNull()
               .WithMessage("The Name Must be Not Null")
               .Length(3, 200)
               .WithMessage("The Name Length Must be between 3 and 200");


            RuleFor(x => x.SenderPhoneNumber)
               .NotEmpty()
               .WithMessage("The Phone Number Must be Not Empty")
               .NotNull()
               .WithMessage("The Phone Number Must be Not Null")
               .Length(11, 11)
               .WithMessage("The Phone Number Length Must be exactly 11 Number")
               .Must(ValidatePhoneNumber)
               .WithMessage("Enter Correct Phone Number");

         

            RuleFor(x => x.Content)
              .NotEmpty()
              .WithMessage("The Message Must be Not Empty")
              .NotNull()
              .WithMessage("The Message Must be Not Null");



        }

        private bool ValidatePhoneNumber(string? phoneNumber)
        {
            if (phoneNumber == null) return false;
            if (phoneNumber[0] != '0' || phoneNumber[1] != '1') return false;

            foreach (var i in phoneNumber)
            {
                if (i < '0' && i > '9') return false;
            }

            return true;
        }

    }
}
