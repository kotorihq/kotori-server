using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Controller for master operations
    /// </summary>
    [Route("api")]
    [Authorize("master")]
    public class MasterController
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
        /// Create project
        /// </summary>
        /// <returns>The operation result message</returns>
        /// <param name="identifier">Identifier of the project</param>
        /// <param name="name">Name of the project</param>
        /// <response code="201">The operation result message</response>
        /// <remarks>Creates the project with an unique identifier which should be a valid slug.</remarks>
        [Route("projects")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<string> CreateProject(string identifier, string name)
        {
            var result = await _kotori.CreateProjectAsync(_instance, identifier, name);

            return result;
        }

        /// <summary>
        /// Get projects
        /// </summary>
        /// <returns>A collection of the projects</returns>
        /// <response code="200">A collection of the projects</response>
        /// <remarks>Gets a collection of existing projects in the instance.</remarks>
        [Route("projects")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleProject>), 200)]
        public async Task<IEnumerable<SimpleProject>> GetProjects()
        {
            var result = await _kotori.GetProjectsAsync(_instance);

            return result;
        }

        /// <summary>
        /// Get project
        /// </summary>
        /// <returns>The project</returns>
        /// <param name="projectId">Project identifier</param>
        /// <response code="200">The project</response>
        /// <remarks>Gets just one project.</remarks>
        [Route("projects/{projectId}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleProject), 200)]
        public async Task<SimpleProject> GetProject(string projectId)
        {
            var result = await _kotori.GetProjectAsync(_instance, projectId);

            return result;
        }

        /// <summary>
        /// Upsert project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="name">Name</param>
        /// <returns>The operation result message</returns>
        /// <response code="201">The operation result message</response>
        /// <remarks>Updates project. Identifier cannot by changed. You can only change a name at the moment.</remarks>
        [Route("projects/{projectId}")]
        [HttpPut]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<string> UpsertProject(string projectId, [FromQuery]string name)
        {
            var result = await _kotori.UpsertProjectAsync(_instance, projectId, name);

            return result;
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
        public async Task<string> DeleteProject(string projectId)
        {
            var result = await _kotori.DeleteProjectAsync(_instance, projectId);

            return result;
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
        public async Task<string> CreateProjectKey(string projectId, [FromQuery, Required]string key, [FromQuery]bool? isReadonly)
        {
            var result = await _kotori.CreateProjectKeyAsync(_instance, projectId, new KotoriCore.Configurations.ProjectKey { Key = key, IsReadonly = isReadonly ?? false });

            return result;
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
        public async Task<string> DeleteProjectKey(string projectId, string key)
        {
            var result = await _kotori.DeleteProjectKeyAsync(_instance, projectId, key);

            return result;
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
        public async Task<string> UpsertProjectKey(string projectId, string key, [FromQuery]bool isReadonly = false)
        {
            var result = await _kotori.UpsertProjectKeyAsync(_instance, projectId, new KotoriCore.Configurations.ProjectKey { Key = key, IsReadonly = isReadonly });

            return result;
        }
    }
}
