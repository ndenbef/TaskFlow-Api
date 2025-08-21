using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
//using MongoDB.Driver;
//using MongoDB.Driver.Linq;
using TaskFlowWebApi.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace TaskFlowWebApi.Services
{
    public class UserServices
    {
        private readonly UserManager<Users> _userManager;
        private readonly IConfiguration _configuration;

        //private readonly IMongoCollection<Users> _usersCollection;


        public UserServices(
            //IOptions<StoreUserDbSettings> storeUserDbSettings, 
            UserManager<Users> userManager, IConfiguration configuration)
        {
            //var mongoClient = new MongoClient(
            //    storeUserDbSettings.Value.ConnectionString);

            //var mongoDatabase = mongoClient.GetDatabase(
            //    storeUserDbSettings.Value.DataBaseName);

            //_usersCollection = mongoDatabase.GetCollection<Users>(
            //    storeUserDbSettings.Value.UsersCollectionName);

            _userManager = userManager;
            _configuration = configuration;

        }

        public Task<List<Users>> GetAsync() =>
            Task.FromResult(_userManager.Users.ToList());

        public async Task<IdentityResult> CreateAsync(Users newUserCreated, string password) =>
            await _userManager.CreateAsync(newUserCreated, password);

        public async Task<Users> GetUserAsync(string Id) =>
            await _userManager.FindByIdAsync(Id);

        public async Task<string?> LoginUserAsync(Login login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: null,
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null;
        }

    }
}
