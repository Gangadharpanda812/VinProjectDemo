namespace FleetGuardAPI.Models
{
    public class ApiError
    {
        public string Status { get; set; } = "error";
        public ErrorDetails Error { get; set; }

        public class ErrorDetails
        {
            public string Code { get; set; }
            public string Message { get; set; }
        }
    }
}
