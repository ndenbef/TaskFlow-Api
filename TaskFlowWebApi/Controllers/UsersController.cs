using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskFlowWebApi.Data;
using TaskFlowWebApi.Models;
using TaskFlowWebApi.Services;

namespace TaskFlowWebApi.Controllers
{
    [ApiController]
    [Route("users/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserServices _userService;

        public UsersController(UserServices userService) =>
            _userService = userService;

        [HttpGet]
        public async Task<List<GetUsers>> GetUsers() 
        {
            var users = await _userService.GetAsync();

            var userFound = users.Select(u => new GetUsers
            {
                FullName = u.FullName,
                Username = u.UserName,
                Email = u.Email,
                CreatedOn = u.CreatedOn,
                LastMod = u.LastMod,
                mode = u.mode,
                pushtoken = u.pushtoken,
                push = u.push,
                ProfileUrl = u.ProfileUrl
            });

            return userFound.ToList();
        }
            

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Register register)
        {
            var newUser = new Users
            {
                FullName = register.FullName,
                UserName = register.Username,
                Email = register.Email,
                ProfileUrl = register.ProfileUrl
            };
            try
            {
                var result = await _userService.CreateAsync(newUser, register.Password);

                if (result.Succeeded)
                    return Ok("User created successfully");
                else 
                {
                    var errors = result.Errors.Select(e => e.Description);

                    return BadRequest(new { Errors = errors });
                }

                    

            }
            catch (Exception ex) 
            {

                return BadRequest($"An error occured: { ex.Message}");
 
            };
            
            
        }       
    }
}
