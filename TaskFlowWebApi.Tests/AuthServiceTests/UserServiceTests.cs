using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskFlowWebApi.Data;
using TaskFlowWebApi.Models;
using TaskFlowWebApi.Services;


namespace TaskFlowWebApi.Tests.AuthServiceTests
{
    internal class UserServiceTests
    {
        //private Mock<IMongoCollection<Users>> _users;
        private Mock<UserManager<Users>> _mockUsersCollection;
        private UserServices _userServices;

        [SetUp]
        public void Setup()
        {
            _mockUsersCollection = new Mock<UserManager<Users>>(
                new Mock<IUserStore<Users>>().Object, null, null, null, null, null, null, null, null);

            //var mockDb = new Mock<IMongoDatabase>();
            //_users = new Mock<IMongoCollection<Users>>();

            _userServices = new UserServices(_mockUsersCollection.Object);
        }

        [Test]
        public async Task CreateAsync_ValidUser_ReturnsSucess()
        {
            //ARRANGE
            var newUser = new Users
            {
                FullName = "Test1",
                UserName = "Test1",
                Email = "Test1",
                ProfileUrl = "Test1"
            };
            var password = "Test.001";

            _mockUsersCollection.Setup(um => um.CreateAsync(newUser, password)).ReturnsAsync(IdentityResult.Success);

            //ACT
            var result = await _userServices.CreateAsync(newUser, password);

            //ASSERT
            Assert.IsTrue(result.Succeeded);
            _mockUsersCollection.Verify(um => um.CreateAsync(newUser, password), Times.Once());
        }

        [Test]
        public async Task GetAsync_GetUsers_ReturnsSuccess()
        {
            //ARRANGE 

            var mockUser = new List<Users>
            {
                new Users { FullName = "Test1", UserName = "Test1", Email = "Test1", LastMod = DateTime.Now, ProfileUrl = "Test1" },
                new Users { FullName = "Test2", UserName = "Test2", Email = "Test2", LastMod = DateTime.Now, ProfileUrl = "Test2" }
            };

            _mockUsersCollection.Setup(um => um.Users).Returns(mockUser.AsQueryable());

            //ACT
            var result = await _userServices.GetAsync();

            //ASSERT
            _mockUsersCollection.Verify(x => x.Users, Times.Once());

            // 2. On vérifie que le nombre d'utilisateurs retournés est correct.
            //Assert.That(mockUser.Count, Is.EqualTo(result.Count));

            Assert.That(mockUser.Count(), Is.EqualTo(result.Count));

            // 3. On vérifie que les utilisateurs retournés sont bien ceux que nous avons simulés.
            Assert.IsTrue(result.All(r => mockUser.Any(mu => mu.Email == r.Email)));
        }

        [Test]
        public async Task GetUser_IsValid_ReturnsSuccess()
        {
            //ARRANGE
            var testId1 = "0f8fad5b-d9cb-469f-a165-70867728950e";
            var testId2 = "0f8fad5b-d9cb-469f-a165-70867728950a";

            var mockUser = new Users { Id = Guid.Parse(testId1), UserName = "Test1", Email = "Test1", LastMod = DateTime.Now, ProfileUrl = "Test1" };

            _mockUsersCollection.Setup(um => um.FindByIdAsync(testId1)).ReturnsAsync(mockUser);

            //ACT
            var result = await _userServices.GetUserAsync(testId1);

            //ASSERT
            _mockUsersCollection.Verify(x => x.FindByIdAsync(testId1), Times.Once());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(Guid.Parse(testId1)));
            Assert.That(result.UserName, Is.EqualTo("Test1"));
        } 
    }
}
