using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Security.Shared;
using TaskFlowProjectApi.Data;
using TaskFlowProjectApi.Models;
using TaskFlowProjectApi.Services;

namespace TaskFlowProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectServices _projectServices;

        public ProjectsController(ProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> createProject([FromBody] CreateProject project)
        {
            
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                {
                    var fulltoken = authorizationHeader.FirstOrDefault();

                    var newProject = new Projects
                    {
                        Id = Guid.NewGuid(),
                        Title = project.Title,
                        Description = project.Description,
                        Deadline = project.Deadline,
                        AllowedHosts = JwtHelper.GetUserIdFromToken(fulltoken)
                    };

                    await _projectServices.createProjectAsync(newProject);

                    return CreatedAtAction(nameof(GetOneProject), new { id = newProject.Id }, newProject);
                }
                return Unauthorized("Veuillez vous authentifier !!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProjects")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Projects))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOneProject(string id)
        {
            try
            {
                var result = await _projectServices.getMyProjectAsync(Guid.Parse(id));

                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Ce Project est introuvable !!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOneProject")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Projects))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var result = await _projectServices.getAllProjectsAsync();

                    return Ok(result.ToList());
                
            }
            catch(Exception ex) 
            {
                return BadRequest($"An error occured: { ex.Message }");
            }
        }
    }
}
