namespace Dr_Home.Helpers.helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T? Data { get; set; }

        public string? token { get; set; }

      
    }
}
