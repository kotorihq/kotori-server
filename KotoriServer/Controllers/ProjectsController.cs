using System.Collections.Generic;
using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Projects controller.
    /// </summary>
    [Route("api/projects")]
    public class ProjectsController
    {
        readonly Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.ProjectsController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public ProjectsController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 202)]
        [Authorize("master")]
        public async Task<string> Post(string name, string identifier, [FromBody]List<ProjectKey> projectKeys = null)
        {
            var result = await _kotori.CreateProjectAsync(_kotori.Configuration.Instance, name, identifier, projectKeys);
            return result;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KotoriCore.Domains.SimpleProject>), 200)]
        [Authorize("master")]
        public async Task<IEnumerable<KotoriCore.Domains.SimpleProject>> Get()
        {
            var result = await _kotori.GetProjectsAsync(_kotori.Configuration.Instance);
            return result;
        }
    }
}