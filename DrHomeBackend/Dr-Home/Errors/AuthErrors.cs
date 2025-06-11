namespace Dr_Home.Errors
{
    public static class AuthErrors
    {
        public static Error WrongVerficationCode = new("Auth.WrongVerficationCode", "The Code Is Wrong Or Expired",
            StatusCodes.Status400BadRequest);

        public static Error AccountAlreadyActive = new("Auth.AccountAlreadyActive", "The Account Is Already Active",
            StatusCodes.Status400BadRequest);

        public static Error WrongForgetPasswordCode = new("Auth.WrongForgetPasswordCode", "The Code Is Wrong Or Expired",
           StatusCodes.Status400BadRequest);
    }
}
