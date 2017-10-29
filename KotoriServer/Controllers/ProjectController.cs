using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Configuration;
using KotoriCore.Exceptions;
using System.Collections.Generic;

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
        public async Task<SimpleProject> GetProjects(string projectId)
        {
            var project = await _kotori.GetProjectAsync(_kotori.Configuration.Instance, projectId);

            return project;
        }

        [Route("document-types")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentType>), 200)]
        public async Task<IEnumerable<SimpleDocumentType>> GetDocumentTypes(string projectId)
        {
            var documentTypes = await _kotori.GetDocumentTypesAsync(_kotori.Configuration.Instance, projectId);

            return documentTypes;
        }
    }
}