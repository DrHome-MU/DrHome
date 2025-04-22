namespace Dr_Home.Errors
{
    public static class ClinicErrors
    {
        public static Error ClinicNotFound = new("Clinic.NotFound", "Clinic Was Not Found With This Id",
            StatusCodes.Status404NotFound);
    }
}
