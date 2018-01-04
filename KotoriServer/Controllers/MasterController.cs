using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
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
        /// <returns>The status code.</returns>
        /// <param name="createProject">Create project request.</param>
        [Route("projects")]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateProject([FromBody]CreateProjectRequest createProject)
        {
            var result = await _kotori.CreateProjectAsync(_instance, createProject.Id, createProject.Name);

            return Created(result.Url, result);
        }

        /// <summary>
        /// Gets projects.
        /// </summary>
        /// <returns>A collection of the projects.</returns>
        [Route("projects")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectResult>), 200)]
        public async Task<IEnumerable<ProjectResult>> GetProjects()
        {
            var result = await _kotori.GetProjectsAsync(_instance);

            return result.Select(x => (ProjectResult)x);
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <returns>The project.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Route("projects/{projectId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ProjectResult), 200)]
        public async Task<ProjectResult> GetProject(string projectId)
        {
            var result = await _kotori.GetProjectAsync(_instance, projectId);

            return result;
        }

        /// <summary>
        /// Upserts the project.
        /// </summary>
        /// <returns>The project.</returns>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="upsertProject">Upsert project request.</param>
        [Route("projects/{projectId}")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProjectResult), 201)]
        public async Task<IActionResult> UpsertProject(string projectId, [FromBody]UpsertProjectRequest upsertProject)
        {
            var result = await _kotori.UpsertProjectAsync(_instance, projectId, upsertProject.Name);

            if (result.NewResource)
                return Created(result.Url, result);
            
            return Ok();
        }

        /// <summary>
        /// Delete project
        /// </summary>
        /// <returns>The operation result message</returns>
        /// <response code="200">The operation result message</response>
        /// <param name="projectId">Project identifier</param>
        /// <remarks>Deletes the project. Do not be worry too much. Non empty projects cannot be deleted.</remarks>
        [Route("projects/{projectId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> DeleteProject(string projectId)
        {
            await _kotori.DeleteProjectAsync(_instance, projectId);

            return NoContent();
        }

        /// <summary>
        /// Get project keys
        /// </summary>
        /// <returns>A collection of project keys</returns>
        /// <response code="200">A collection of project keys</response>
        /// <param name="projectId">Project identifier</param>
        /// <remarks>Gets project keys.</remarks>
        [Route("projects/{projectId}/project-keys")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectKey>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<ProjectKey>> GetProjectKeys(string projectId)
        {
            var projectKeys = await _kotori.GetProjectKeysAsync(_instance, projectId);

            return projectKeys;
        }

        /// <summary>
        /// Create project key
        /// </summary>
        /// <returns>The operation result message</returns>
        /// <param name="projectId">Project identifier</param>
        /// <param name="key">Key</param>
        /// <param name="isReadonly">Flag if it is readonly key</param>
        /// <response code="201">The operation result message</response>
        /// <remarks>Creates a new project key.</remarks>
        [Route("projects/{projectId}/project-keys")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> CreateProjectKey(string projectId, [FromQuery, Required]string key, [FromQuery]bool? isReadonly)
        {
            var result = await _kotori.CreateProjectKeyAsync(_instance, projectId, new KotoriCore.Configurations.ProjectKey { Key = key, IsReadonly = isReadonly ?? false });

            return Created(result.Url, result);
        }

        /// <summary>
        /// Delete project key
        /// </summary>
        /// <returns>The operation result message</returns>
        /// <param name="projectId">Project identifier</param>
        /// <param name="key">Key</param>
        /// <response code="200">The operation result message</response>
        /// <remarks>Deletes an existing project key.</remarks>
        [Route("projects/{projectId}/project-keys/{key}")]
        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> DeleteProjectKey(string projectId, string key)
        {
            await _kotori.DeleteProjectKeyAsync(_instance, projectId, key);

            return NoContent();
        }

        /// <summary>
        /// Upsert project key
        /// </summary>
        /// <returns>The operation result message</returns>
        /// <param name="projectId">Project identifier</param>
        /// <param name="key">Key</param>
        /// <param name="isReadonly">If set to <c>true</c> is readonly</param>
        /// <response code="200">The operation result message</response>
        /// <remarks>Update an existing project key. You can just change if it's readonly or not.</remarks>
        [Route("projects/{projectId}/project-keys/{key}")]
        [HttpPut]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpsertProjectKey(string projectId, string key, [FromQuery]bool isReadonly = false)
        {
            var result = await _kotori.UpsertProjectKeyAsync(_instance, projectId, new KotoriCore.Configurations.ProjectKey { Key = key, IsReadonly = isReadonly });

            return Ok(result);
        }
    }
}
