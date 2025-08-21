using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
//using MongoDB.Driver;
//using MongoDB.Driver.Linq;
using TaskFlowWebApi.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TaskFlowWebApi.Services
{
    public class UserServices
    {
        private readonly UserManager<Users> _userManager;

        //private readonly IMongoCollection<Users> _usersCollection;


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

        public Task<List<Users>> GetAsync() =>
            Task.FromResult(_userManager.Users.ToList());

        public async Task<IdentityResult> CreateAsync(Users newUserCreated, string password) =>
            await _userManager.CreateAsync(newUserCreated, password);

        public async Task<Users> GetUserAsync(string Id) =>
            await _userManager.FindByIdAsync(Id);

    }
}
