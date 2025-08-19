namespace TaskFlowWebApi.Models
{
    public class StoreUserDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DataBaseName { get; set; } = string.Empty;
        public string UsersCollectionName { get; set; } = string.Empty;
    }
}
