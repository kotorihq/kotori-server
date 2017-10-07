using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Configuration;
using KotoriCore.Exceptions;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Project controller.
    /// </summary>
    [Route("api/projects/{projectId}")]
    public class ProjectController
    {
        readonly Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.ProjectController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public ProjectController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SimpleProject), 200)]
        [Authorize("master")]
        public async Task<SimpleProject> Get(string projectId)
        {
            var result = await _kotori.GetProjectsAsync(_kotori.Configuration.Instance);

            var project = result.FirstOrDefault(p => p.Identifier.Equals(projectId));

            if (project == null)
                throw new KotoriProjectException(projectId, "Project not found.");
            
            return project;
        }
    }
}