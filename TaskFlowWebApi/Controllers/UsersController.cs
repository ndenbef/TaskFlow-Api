using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TaskFlowWebApi.Data;
using TaskFlowWebApi.Models;
using TaskFlowWebApi.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [Authorize]
        public async Task<List<GetUsers>> GetUsers() 
        {
            var users = await _userService.GetAsync();

            var userFound = users.Select(u => new GetUsers
            {
                Id = u.Id,
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
                    return Created();
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

        [HttpGet("GetUserbyId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUsers))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserbyId(string Id)
        {
            if(Id != null)
            {
                var result = await _userService.GetUserAsync(Id);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("User not found");
                }
            }else
            {
                return BadRequest("Please choose a user");
            }
            
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] Login login)
        {

            try
            {
                var result = await _userService.LoginUserAsync(login);
                

                if (result != null)
                {

                    return Ok(new { message = "User Logged Sucessfully!", token = result });
                }
                return BadRequest("Nom d'utilisateur ou Mot de Passe Invalide");
            }
            catch(Exception ex)
                {
                    return BadRequest($"An error occured : {ex.Message}");

                }




}
    }
}
