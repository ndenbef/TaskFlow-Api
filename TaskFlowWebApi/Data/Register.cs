namespace TaskFlowWebApi.Models
{
    public class Register
    {
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get;  set; } = string.Empty;
        public string? ProfileUrl { get; set; } = string.Empty;
    }
}
