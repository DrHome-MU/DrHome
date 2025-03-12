namespace Dr_Home.Settings
{
    public static class FileSettings
    {
        public const int MaximumSizeInMb = 1;

        public const int MaximumSizeInBytes = MaximumSizeInMb * 1024 * 1024;

        public static readonly string[] BlockedSignatures = ["4D,5A","2F-2A","D0-CF"];

        public static readonly string[] AllowedExtensions = [".jpg" , ".jpeg" , ".png"];
    }
}
