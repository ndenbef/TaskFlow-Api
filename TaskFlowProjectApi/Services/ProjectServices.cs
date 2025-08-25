using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskFlowProjectApi.Models;

namespace TaskFlowProjectApi.Services
{
    public class ProjectServices
    {
        private readonly IMongoCollection<Projects> _projectsCollection;

        public ProjectServices(IOptions<StoreProjectDbSettings> storeProjectDbSettings)
        {
            var mongoClient = new MongoClient(
                storeProjectDbSettings.Value.ConnectionString);

            var mongoDataBase = mongoClient.GetDatabase(
                storeProjectDbSettings.Value.DataBaseName);

            _projectsCollection = mongoDataBase.GetCollection<Projects>(
                storeProjectDbSettings.Value.ProjectCollectionName);
        }

        public async Task createProjectAsync(Projects projects) =>
            await _projectsCollection.InsertOneAsync(projects);

        public async Task<List<Projects>> getAllProjectsAsync() =>
            await _projectsCollection.Find(_ => true).ToListAsync();

        public async Task<List<Projects>> getAllMyProjectsAsync(Guid id) =>
            await _projectsCollection.Find(x => x.OwnerId == id).ToListAsync();

        public async Task<Projects> getMyProjectAsync(Guid id) =>
            await _projectsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task updateProjectAsync(Guid id, UpdateDefinition<Projects> projectToUpdate) =>
            await _projectsCollection.UpdateOneAsync(x => x.Id == id, projectToUpdate);

        public async Task removeProjectAsync(Guid id) =>
            await _projectsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
