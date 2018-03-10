using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KotoriCore;
using KotoriServer.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Controller for master operations.
    /// </summary>
    [Route("api")]
    [EnableCors("AllowAnyOrigin")]
    [Authorize("master")]
    public class MasterController : ControllerBase
    {
        readonly Kotori _kotori;
        readonly string _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.MasterController"/> class.
        /// </summary>
        /// <param name="kotori">Kotori.</param>
        public MasterController(IKotori kotori)
        {
            _kotori = kotori as Kotori;
            _instance = kotori.Configuration.Instance;
        }

        /// <summary>
        /// Creates the project.
        /// </summary>
        /// <returns>The create project result.</returns>
        /// <param name="createProject">Create project request.</param>
        [Route("projects")]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody]CreateProjectRequest createProject)
        {
            var result = await _kotori.CreateProjectAsync(_instance, null, createProject.Name);

            return Created(result.Url, new { id = result.Id, url = result.Url });
        }

        /// <summary>
        /// Gets projects.
        /// </summary>
        /// <returns>A collection of the projects.</returns>
        [Route("projects")]
        [HttpGet]
        public async Task<ComplexCountResult<ProjectResult>> GetProjects()
        {
            var projects = await _kotori.GetProjectsAsync(_instance);

            return new ComplexCountResult<ProjectResult>(projects.Count, projects.Items.Select(p => new ProjectResult(p.Identifier, p.Name)));
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <returns>The project.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Route("projects/{projectId}")]
        [HttpGet]
        public async Task<ProjectResult> GetProject(string projectId)
        {
            var result = await _kotori.GetProjectAsync(_instance, projectId);

            return result;
        }

        /// <summary>
        /// Upserts the project.
        /// </summary>
        /// <returns>No content or create project result.</returns>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="upsertProject">Upsert project request.</param>
        [Route("projects/{projectId}")]
        [HttpPut]
        public async Task<IActionResult> UpsertProject([Required]string projectId, [FromBody]UpsertProjectRequest upsertProject)
        {
            var result = await _kotori.UpsertProjectAsync(_instance, projectId, upsertProject.Name);

            if (result.NewResource)
                return Created(result.Url, new { id = result.Id, url = result.Url });
            
            return Ok();
        }


        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <returns>No content.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Route("projects/{projectId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProject(string projectId)
        {
            await _kotori.DeleteProjectAsync(_instance, projectId);

            return NoContent();
        }

        /// <summary>
        /// Gets the project keys.
        /// </summary>
        /// <returns>The project keys.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Route("projects/{projectId}/project-keys")]
        [HttpGet]
        public async Task<Tokens.ComplexCountResult<ProjectKeyResult>> GetProjectKeys(string projectId)
        {
            var projectKeys = await _kotori.GetProjectKeysAsync(_instance, projectId);

            return new Tokens.ComplexCountResult<ProjectKeyResult>(projectKeys.Count, projectKeys.Items.Select(pk => new ProjectKeyResult(pk.Key, pk.IsReadonly)));
        }

        /// <summary>
        /// Creates project key.
        /// </summary>
        /// <returns>The operation result.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Route("projects/{projectId}/project-keys")]
        [HttpPost]
        public async Task<IActionResult> CreateProjectKey(string projectId, [FromBody]CreateProjectKeyRequest createProjectKey)
        {
            var result = await _kotori.CreateProjectKeyAsync(_instance, projectId, new KotoriCore.Configurations.ProjectKey { IsReadonly = createProjectKey.IsReadonly ?? false });

            return Created(result.Url, new { id = result.Id, url = result.Url });
        }

        /// <summary>
        /// Deletes the project key.
        /// </summary>
        /// <returns>The project key.</returns>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="key">Key.</param>
        [Route("projects/{projectId}/project-keys/{key}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProjectKey(string projectId, [Required]string key)
        {
            await _kotori.DeleteProjectKeyAsync(_instance, projectId, key);

            return NoContent();
        }

        /// <summary>
        /// Upserts the project key.
        /// </summary>
        /// <returns>Upsert project key result.</returns>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="createProjectKey">Create project key request.</param>
        [Route("projects/{projectId}/project-keys/{key}")]
        [HttpPut]
        public async Task<IActionResult> UpsertProjectKey(string projectId, [Required]string key, [FromBody]CreateProjectKeyRequest createProjectKey)
        {
            var result = await _kotori.UpsertProjectKeyAsync(_instance, projectId, new KotoriCore.Configurations.ProjectKey { Key = key, IsReadonly = createProjectKey.IsReadonly ?? false });

            if (result.NewResource)
                return Created(result.Url, new { id = result.Id, url = result.Url });

            return Ok();
        }
    }
}