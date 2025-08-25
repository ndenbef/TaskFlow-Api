using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskFlowTaskApi.Models;

namespace TaskFlowTaskApi.Services
{
    public class TaskServices
    {
        private readonly IMongoCollection<Task> _taskCollection;

        public TaskServices (IOptions<StoreTasksDbSettings> storeTasksDbSettings)
        {
            var mongoClient = new
        }
    }
}
