namespace TaskFlowWebApi.Data
{
    public class GetUsers
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastMod { get; set; } = DateTime.UtcNow;
        public string mode { get; set; } = string.Empty;
        public string pushtoken { get; set; } = string.Empty;
        public string push { get; set; } = string.Empty;
        public string? ProfileUrl { get; set; } = string.Empty;
    }
}
