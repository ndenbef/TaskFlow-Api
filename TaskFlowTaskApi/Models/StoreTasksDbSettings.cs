namespace TaskFlowTaskApi.Models
{
    public class StoreTasksDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string TaskCollectionName { get; set; } = string.Empty;
    }
}
