using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TaskFlowWebApi.Models;

namespace TaskFlowWebApi.Services
{
    public class UserServices
    {
        private readonly UserManager<Users> _userManager;

        private readonly IMongoCollection<Users> _usersCollection;


        public UserServices(
            //IOptions<StoreUserDbSettings> storeUserDbSettings, 
            UserManager<Users> userManager)
        {
            //var mongoClient = new MongoClient(
            //    storeUserDbSettings.Value.ConnectionString);

            //var mongoDatabase = mongoClient.GetDatabase(
            //    storeUserDbSettings.Value.DataBaseName);

            //_usersCollection = mongoDatabase.GetCollection<Users>(
            //    storeUserDbSettings.Value.UsersCollectionName);

            _userManager = userManager;

        }

        public async Task<List<Users>> GetAsync() =>
            await _userManager.Users.ToListAsync();

        public async Task<IdentityResult> CreateAsync(Users newUserCreated, string password) =>
            await _userManager.CreateAsync(newUserCreated, password);


    }
}
