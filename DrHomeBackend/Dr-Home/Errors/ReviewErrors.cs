namespace Dr_Home.Errors
{
    public static class ReviewErrors
    {
        public static Error ReviewNotFound = new("Review.NotFound", "Review Was Not Found With This Id",
           StatusCodes.Status404NotFound);

        public static Error PatientMadeReviewBefore = new("Review.Conflict", "Patient Made Review Before To This Doctor",
           StatusCodes.Status409Conflict);

        public static Error UnauthorizedUpdateReview = new("Review.Unauthorized", "This Patient Can not update This Review",
          StatusCodes.Status401Unauthorized);
    }
}
