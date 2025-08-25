namespace TaskFlowProjectApi.Data
{
    public class AddProjectMember
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? AllowedHosts { get; set; }
    }
}
