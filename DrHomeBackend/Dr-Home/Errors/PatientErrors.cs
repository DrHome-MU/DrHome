namespace Dr_Home.Errors
{
    public static class PatientErrors
    {
        public static Error PatientNotFound = new("Patient.NotFound", "Patient Was Not Found With This Id",
           StatusCodes.Status404NotFound);
    }
}
