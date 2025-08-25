using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
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
                        ParentProject = project.ParentProject,
                        Title = project.Title,
                        Description = project.Description,
                        Deadline = project.Deadline,
                        AllowedHosts = JwtHelper.GetUserIdFromToken(fulltoken).ToString()
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

        [HttpGet("MyProjects")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Projects))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMyProjects()
        {
            try
            {
                if(Request.Headers.TryGetValue("Authorization", out var authorisationHeader))
                {
                    var fulltoken = authorisationHeader.FirstOrDefault();

                    var userid = JwtHelper.GetUserIdFromToken(fulltoken);

                    var myProjects = await _projectServices.getAllMyProjectsAsync(Guid.Parse(userid));

                    if(myProjects != null)
                    {
                        return Ok(myProjects);
                    }
                    return Ok("You don't have any projects yet !!");
                }
                return Unauthorized("You are not authenticated !!");
            }
            catch(Exception ex)
            {
                return BadRequest($"An error occured : {ex.Message}");
            }
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProjectMember([FromBody] AddProjectMember projectMember)
        {
            try
            {
                var result = await _projectServices.getMyProjectAsync(Guid.Parse(projectMember.Id));
                if(result != null)
                {
                    return NotFound("Project not found!!");
                }
                string updatedAllowedHosts = string.IsNullOrEmpty(result.AllowedHosts) ? projectMember.AllowedHosts : $"{result.AllowedHosts},{projectMember.AllowedHosts}";

                var update = Builders<Projects>.Update.Set(u => u.AllowedHosts, updatedAllowedHosts);

                await _projectServices.updateProjectAsync(Guid.Parse(projectMember.Id), updatedAllowedHosts);

                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest($"Une erreur s'est produite {ex.Message}");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteOneProject([FromBody] string id)
        {
            try
            {
                 await _projectServices.removeProjectAsync(Guid.Parse(id));

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest($"An error occured : {ex.Message}");
            }
            
        }


    }
}
