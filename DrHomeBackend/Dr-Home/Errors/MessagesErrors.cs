namespace Dr_Home.Errors
{
    public static class MessagesErrors
    {
        public static Error DuplicateMessage = new("Auth.DuplicateMessage", "You Sent This Message Before.",
            StatusCodes.Status400BadRequest);

        public static Error MessageNotFound = new("Auth. MessageNotFound", "Ther Is No Message With This Id.",
           StatusCodes.Status404NotFound);
    }
}
